using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using TauCode.Extensions;

namespace TauCode.Parsing.Utility.Tests
{
    public enum Phase
    {
        Local = 1,
        Host = 2,
    }

    public ref struct Watush
    {
        public int A;
    }


    [TestFixture]
    public class TodoEmailFixture
    {
        public void GeFoo(ref Watush w)
        {
            w.A = 1;
        }

        public bool IsEmail(string input)
        {
            Watush w = new Watush();
            GeFoo(ref w);


            var k = 3;


            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.Length == 0)
            {
                return false;
            }

            const int maxLen = 254;
            const int localPartMaxLen = 64;

            if (input.Length > maxLen)
            {
                return false;
            }

            var length = input.Length;
            var index = 0;
            var phase = Phase.Local;

            // 1. local part
            char? prevChar = null;
            while (true)
            {
                if (index == localPartMaxLen)
                {
                    return false;
                }

                if (index == length)
                {
                    return false; // never got '@'
                }

                var c = input[index];
                if (char.IsLetterOrDigit(c))
                {
                    // ok.
                    prevChar = c;
                    index++;
                    continue;
                }

                if (c == '@')
                {
                    if (prevChar == '.' || index == 0)
                    {
                        return false; // local part must not end with '.'; local part cannot be empty.
                    }

                    // don't need to update prevChar
                    index++;
                    break;
                }

                if (c == '.')
                {
                    if (prevChar == '.' || index == 0)
                    {
                        return false; // cannot have two '.'s in a row; cannot start with '.'
                    }

                    prevChar = c;
                    index++;
                    continue;
                }

                if (
                    c == '-' ||
                    c == '+' ||
                    c == '!' ||
                    c == '%' ||
                    c == '~' ||
                    c == '$' ||
                    false // todo: optimize with hashset?
                )
                {
                    // accepted char
                    prevChar = c;
                    index++;
                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    return false; // no spaces in local part
                }

                throw new NotImplementedException();
            }

            // 2. host
            var hostStart = index;
            prevChar = null;

            while (true)
            {
                if (index == length)
                {
                    if (
                        prevChar == '.' ||
                        prevChar == '-'
                        )
                    {
                        return false; // host cannot end with '.' or '-'
                    }

                    break;
                }

                var c = input[index];

                if (char.IsLetterOrDigit(c))
                {
                    index++;
                    prevChar = c;
                    continue;
                }

                if (c == '.')
                {
                    if (
                        prevChar == '.' ||
                        prevChar == '-' ||
                        false
                        )
                    {
                        return false; // host cannot contain substrings '-.' or '..'
                    }

                    index++;
                    prevChar = c;
                    continue;
                }

                if (c == '-')
                {
                    if (index == 0)
                    {
                        return false; // host cannot start with '-'
                    }

                    index++;
                    prevChar = c;
                    continue;
                }


                throw new NotImplementedException();
            }

            var hostEnd = index;
            //var hostSpan = input.AsSpan(hostStart);

            var hostString = input.Substring(hostStart);
            var res = Uri.CheckHostName(hostString);
            return
                res == UriHostNameType.Dns ||
                res == UriHostNameType.IPv4 ||
                res == UriHostNameType.IPv6;
        }

        [Test]
        public void TodoWat()
        {
            var str = this.GetType().Assembly.GetResourceText("TestCases.xml", true);
            var xml = new XmlDocument();
            xml.LoadXml(str);

            var divs = xml.SelectNodes("//div[@class='isemail isemail_tooltip']").Cast<XmlElement>().ToList();
            var list = new List<TestCaseDto>();

            foreach (var div in divs)
            {
                var span = div.SelectSingleNode("span");
                if (span == null)
                {
                    continue;
                }


                var testName = ((XmlText)(span.ChildNodes[0])).InnerText.Trim();

                var email = span.SelectSingleNode("strong").InnerText;
                email = ReplaceHtmlEscapeCodes(email);

                var expectedResultString = span.ChildNodes[4].InnerText.Split(':')[1].Trim();
                bool expectedResult;
                switch (expectedResultString)
                {
                    case "Valid":
                        expectedResult = true;
                        break;

                    case "Invalid":
                        expectedResult = false;
                        break;

                    default:
                        throw new Exception();
                }

                var comment = span.ChildNodes[6].InnerText.Trim();

                var testCase = new TestCaseDto(
                    testName,
                    email,
                    expectedResult,
                    comment);
                list.Add(testCase);
            }

            var json = JsonConvert.SerializeObject(list, Newtonsoft.Json.Formatting.Indented);

            throw new NotImplementedException();
        }

        [TestCaseSource(nameof(TestCases))]
        public void TestTodoHere(TestCaseDto testCase)
        {
            var isEmail = IsEmail(testCase.Email);

            Assert.That(isEmail, Is.EqualTo(testCase.ExpectedResult));
        }

        private static string ReplaceHtmlEscapeCodes(string input)
        {
            var res = Regex.Replace(input, @"&#\d\d;", HtmlReplace);
            return res;
        }

        private static string HtmlReplace(Match match)
        {
            switch (match.Value)
            {
                case "&#13;":
                    return "\r";

                case "&#10;":
                    return "\n";

                default:
                    throw new Exception();
            }
        }

        public static IList<TestCaseDto> TestCases
        {
            get
            {
                var json = typeof(TodoEmailFixture).Assembly.GetResourceText("TestCases.json", true);
                var list = JsonConvert.DeserializeObject<IList<TestCaseDto>>(json);
                return list;
            }
        }
    }
}
