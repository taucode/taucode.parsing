using NUnit.Framework;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    [TestFixture]
    public class StringExtractorTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new /*CliLexer()*/ CliOmicronLexer();
        }

        [Test]
        public void Lexize_UnclosedString_ThrowsLexingException()
        {
            // Arrange
            var input = "\n \"unclosed";

            // Act
            var ex = Assert.Throws<LexingException>(() => _lexer.Lexize(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unclosed string."));
            Assert.That(ex.Position, Is.EqualTo(new Position(1, 10)));
        }
    }
}
