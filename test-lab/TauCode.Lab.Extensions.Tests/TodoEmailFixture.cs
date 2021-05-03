using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;
using TauCode.Extensions;

namespace TauCode.Lab.Extensions.Tests
{
    public enum Phase
    {
        Local = 1,
        Host = 2,
    }



    public struct Watush
    {
        public int A;
    }


    [TestFixture]
    public class TodoEmailFixture
    {
        private static readonly HashSet<char> IPv6AcceptableChars;

        private static readonly int IPv6MaxLength = "eeee:eeee:eeee:eeee:eeee:eeee:eeee:eeee:111.111.111.111".Length;

        private static readonly int IPv4MinLength = "1.1.1.1".Length;
        private static readonly int IPv4MaxLength = "101.101.101.101".Length;
        
        static TodoEmailFixture()
        {
            var list = new List<char>();
            list.AddCharRange('a', 'f');
            list.AddCharRange('A', 'F');
            list.AddCharRange('0', '9');
            list.Add('.');
            list.Add(':');

            IPv6AcceptableChars = list.ToHashSet();
        }

        public void GeFoo(ref Watush w)
        {
            w.A = 1;
        }

        public bool IsEmail(string input)
        {
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

            // 1. local part
            char? prevChar = null;
            bool onlySpacesAllowed = false;

            while (true)
            {
                if (index > localPartMaxLen)
                {
                    return false; // too long local part
                }

                if (index == length)
                {
                    return false; // never got '@'
                }

                var c = input[index];
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    if (onlySpacesAllowed)
                    {
                        return false;
                    }

                    // ok.
                    prevChar = c;
                    index++;
                    continue;
                }

                if (c == '@')
                {
                    if (
                        prevChar == '.' ||
                        prevChar == '\\' ||
                        index == 0 ||
                        false
                        )
                    {
                        return false; // local part must not end with '.'; local part cannot be empty.
                    }

                    // don't need to update prevChar
                    index++;
                    break;
                }

                if (c == '"')
                {
                    if (onlySpacesAllowed)
                    {
                        return false;
                    }

                    index++;

                    var skippedOk = SkipQuotedString(input, ref index, ref prevChar);
                    if (!skippedOk)
                    {
                        return false;
                    }

                    continue;
                }

                if (c == '.')
                {
                    if (onlySpacesAllowed)
                    {
                        return false;
                    }

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
                    c == '=' ||
                    c == '!' ||
                    c == '?' ||
                    c == '%' ||
                    c == '~' ||
                    c == '$' ||
                    c == '&' ||
                    c == '/' ||
                    c == '|' ||
                    c == '{' ||
                    c == '}' ||
                    c == '#' ||
                    c == '*' ||
                    c == '\'' ||
                    false // todo: optimize with hashset?
                )
                {
                    if (onlySpacesAllowed)
                    {
                        return false;
                    }

                    // accepted char
                    prevChar = c;
                    index++;
                    continue;
                }

                if (c == '\\')
                {
                    if (onlySpacesAllowed)
                    {
                        return false;
                    }

                    if (prevChar == '.')
                    {
                        return false; // no escapes after '.'
                    }

                    // got escape
                    prevChar = c;
                    index++;
                    continue;
                }

                if (c == '(')
                {
                    if (onlySpacesAllowed)
                    {
                        return false;
                    }
                    
                    index++;
                    var skippedOk = SkipComment(input, ref index);
                    if (!skippedOk)
                    {
                        return false;
                    }

                    continue;
                }

                if (char.IsWhiteSpace(c))
                {
                    onlySpacesAllowed = true;
                    prevChar = c;
                    index++;
                    continue;
                }

                return false;
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

                if (c == '(')
                {
                    if (index == hostStart)
                    {
                        // can have comments at beginning
                        index++;
                        var commentSkipped = SkipComment(input, ref index);

                        if (!commentSkipped)
                        {
                            return false;
                        }

                        hostStart = index;
                    }

                    return false;
                }

                if (c == '[')
                {
                    // todo: comment can follow host
                    // todo: make sure that there is no crap after host
                    if (index == hostStart)
                    {
                        // seems like we've got IP-address-like host
                        index++;

                        if (index < input.Length - 1 && char.IsDigit(input[index + 1]))
                        {
                            var isIPv4Addr = SkipIpv4Address(input, ref index);
                            if (!isIPv4Addr)
                            {
                                return false;
                            }

                            var isEnd = index == input.Length;
                            return isEnd;
                        }

                        var result = SkipIpv6Address(input, ref index);
                        return result;
                    }
                    else
                    {
                        return false;
                    }
                }

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

                return false; // unexpected char for host
            }

            var hostString = input.Substring(hostStart);
            var res = Uri.CheckHostName(hostString);
            return
                res == UriHostNameType.Dns ||
                res == UriHostNameType.IPv4 ||
                res == UriHostNameType.IPv6;
        }

        private bool SkipComment(string input, ref int index)
        {
            var depth = 1;

            while (true)
            {
                if (index == input.Length)
                {
                    return false;
                }

                var c = input[index];
                if (c == '(')
                {
                    index++;
                    depth++;
                    continue;
                }

                if (c == ')')
                {
                    index++;
                    depth--;
                    if (depth == 0)
                    {
                        return true;
                    }

                    continue;
                }

                if (c == '\\')
                {
                    // got escaping
                    index++;
                    if (index == input.Length)
                    {
                        return false; // nothing escaped
                    }

                    c = input[index];
                    if (c == '(' || c == ')')
                    {
                        index++; // escaped parenthesis
                    }

                    continue;
                }

                index++;
            }
        }

        private bool SkipQuotedString(string input, ref int index, ref char? closingChar)
        {
            var start = index;
            while (true)
            {
                if (index == input.Length)
                {
                    return false;
                }

                var c = input[index];
                if (c == '"')
                {
                    index++;
                    closingChar = c;
                    break;
                }

                if (c == '\\')
                {
                    // got escaping
                    index++;

                    if (index == input.Length)
                    {
                        return false; // quoted nothing
                    }

                    c = input[index];
                    if (c == '\\' || c == '"')
                    {
                        index++;
                        continue;
                    }
                }

                if (c == '\r')
                {
                    return false;
                }

                index++;
            }

            var strLen = index - start - 1;
            return strLen > 0;
        }

        private bool SkipIpv4Address(string input, ref int index)
        {
            var remaining = input.Length - index;
            if (remaining < IPv4MinLength + 1) // +1 because ']' must be
            {
                return false;
            }

            var start = index;
            int delta;

            while (true)
            {
                if (index == input.Length)
                {
                    return false;
                }

                delta = index - start;
                if (delta > IPv4MaxLength)
                {
                    return false;
                }

                var c = input[index];
                if (c == '.' || char.IsDigit(c))
                {
                    index++;
                    continue;
                }

                if (c == ']')
                {
                    index++;
                    break;
                }

                return false;
            }

            delta = index - start - 1; // '-1' because not include ']'
            var span = input.AsSpan(start, delta);
            var parsed = IPAddress.TryParse(span, out var address);
            if (parsed)
            {
                return address.AddressFamily == AddressFamily.InterNetwork;
            }

            return false;
        }

        private bool SkipIpv6Address(string input, ref int index)
        {
            var remaining = input.Length - index;

            if (remaining >= "IPv6:".Length)
            {
                // todo optimize
                if (input.Substring(index, "IPv6:".Length) == "IPv6:")
                {
                    index += "IPv6:".Length;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            var start = index;
            int delta;

            while (true)
            {
                if (index == input.Length)
                {
                    return false;
                }

                delta = index - start;
                if (delta > IPv6MaxLength)
                {
                    return false;
                }

                var c = input[index];
                if (IPv6AcceptableChars.Contains(c))
                {
                    index++;
                    continue;
                }

                if (c == ']')
                {
                    index++;
                    break;
                }

                return false;
            }

            delta = index - start - 1; // '-1' because not include ']'
            var span = input.AsSpan(start, delta);
            var parsed = IPAddress.TryParse(span, out var address);
            if (parsed)
            {
                return address.AddressFamily == AddressFamily.InterNetworkV6;
            }

            return false;
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

        //[TestCaseSource(nameof(TestCases))]
        //public void TestTodoHere(TestCaseDto testCase)
        //{
        //    var isEmail = IsEmail(testCase.Email);
        //    Assert.That(isEmail, Is.EqualTo(testCase.ExpectedResult));
        //}

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
