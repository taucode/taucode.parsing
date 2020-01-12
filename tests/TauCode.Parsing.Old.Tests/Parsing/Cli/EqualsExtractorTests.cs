using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Old.Tests.Parsing.Cli
{
    [TestFixture]
    public class EqualsExtractorTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new OldCliLexer();
        }

        [Test]
        [TestCase("\r \n=", 2, 0)]
        [TestCase("\r \n= \r\n \n", 2, 0)]
        [TestCase("\r\n = \r\n \n", 1, 1)]
        public void Lexize_EqualsSignSurroundedByAnyWhiteSpace_PunctuationTokenIsExtracted(string input, int line, int column)
        {
            // Arrange

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(1));
            var token = (PunctuationToken)tokens.Single();
            Assert.That(token.Value, Is.EqualTo('='));
            Assert.That(token.Position, Is.EqualTo(new Position(line, column)));
            Assert.That(token.ConsumedLength, Is.EqualTo(1));
        }
    }
}
