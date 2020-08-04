using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Exceptions;
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
            // Arrange
            var input = "\"\\n\\v\\t\\r\\a\\b\\0\\t\\f\\\\\\\"\"";
            ILexer lexer = new MyStringLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n\v\t\r\a\b\0\t\f\\\""));
        }

        [Test]
        public void EscapeString_MixedEscapes_EscapesCorrectly()
        {
            // Arrange
            var input = "\"\\n\\v\\t\\r\\a\\b\\0\\t\\f\\\\\\u1488\"";
            ILexer lexer = new MyStringLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n\v\t\r\a\b\0\t\f\\\u1488"));
        }

        [Test]
        public void Lexize_UnclosedString_ThrowsLexingException()
        {
            // Arrange
            var input = " \"Unclosed string";
            ILexer lexer = new MyStringLexer();

            // Act
            var ex = Assert.Throws<LexingException>(() => lexer.Lexize(input));
            
            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unclosed string."));
            Assert.That(ex.Position, Is.EqualTo(new Position(0, 17)));
        }

        [Test]
        [TestCase("\"Broken string\n")]
        [TestCase("\"Broken string\r")]
        [TestCase("\"Broken string\n\r")]
        [TestCase("\"Broken string\r\n")]
        public void Lexize_NewLineInString_ThrowsLexingException(string input)
        {
            // Arrange
            ILexer lexer = new MyStringLexer();

            // Act
            var ex = Assert.Throws<LexingException>(() => lexer.Lexize(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Newline in string constant."));
            Assert.That(ex.Position, Is.EqualTo(new Position(0, 14)));
        }

        [Test]
        [TestCase("\"End after escape\\")]
        public void Lexize_EndAfterEscape_ThrowsLexingException(string input)
        {
            // Arrange
            ILexer lexer = new MyStringLexer();

            // Act
            var ex = Assert.Throws<LexingException>(() => lexer.Lexize(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unclosed string."));
            Assert.That(ex.Position, Is.EqualTo(new Position(0, 18)));
        }

        [Test]
        [TestCase("\"Bad\\o\"")]
        public void Lexize_WrongSingleCharEscape_ThrowsLexingException(string input)
        {
            // Arrange
            ILexer lexer = new MyStringLexer();

            // Act
            var ex = Assert.Throws<LexingException>(() => lexer.Lexize(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad escape."));
            Assert.That(ex.Position, Is.EqualTo(new Position(0, 4)));
        }

        [Test]
        [TestCase("\"Bad\\u")]
        [TestCase("\"Bad\\u1")]
        [TestCase("\"Bad\\u11")]
        [TestCase("\"Bad\\u111")]
        [TestCase("\"Bad\\u111z\"")]
        public void Lexize_BadUEscape_ThrowsLexingException(string input)
        {
            // Arrange
            ILexer lexer = new MyStringLexer();

            // Act
            var ex = Assert.Throws<LexingException>(() => lexer.Lexize(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Bad escape."));
            Assert.That(ex.Position, Is.EqualTo(new Position(0, 4)));
        }

    }
}
