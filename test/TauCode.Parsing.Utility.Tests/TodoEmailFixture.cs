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

            if (input.Length > maxLen)
            {
                return false;
            }

            var length = input.Length;
            var index = 0;
            var phase = Phase.Local;

            // 1. local part
            while (true)
            {
                if (index == length)
                {
                    return false; // never got '@'
                }

                var c = input[index];
                if (char.IsLetterOrDigit(c))
                {
                    // ok.

                    index++;
                    continue;
                }

                if (c == '@')
                {
                    index++;
                    break;
                }

                throw new NotImplementedException();
            }

            // 2. host
            var hostStart = index;
            while (true)
            {
                if (index == length)
                {
                    break;
                }

                var c = input[index];

                throw new NotImplementedException();
            }

            throw new NotImplementedException();
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
