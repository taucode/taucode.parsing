using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Lexing
{
    [TestFixture]
    public class StringLexingTests
    {
        [Test]
        public void EscapeString_SingleCharEscape_EscapesCorrectly()
        {
            var input = "\"\\n\\v\\t\\r\\a\\b\\0\\t\\f\\\\\\\"\"";
            ILexer lexer = new MyStringLexer();
            var tokens = lexer.Lexize(input);

            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n\v\t\r\a\b\0\t\f\\\""));
        }

        [Test]
        public void EscapeString_MixedEscapes_EscapesCorrectly()
        {
            var input = "\"\\n\\v\\t\\r\\a\\b\\0\\t\\f\\\\\\u1488\"";
            ILexer lexer = new MyStringLexer();
            var tokens = lexer.Lexize(input);

            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n\v\t\r\a\b\0\t\f\\\u1488"));
        }
    }
}
