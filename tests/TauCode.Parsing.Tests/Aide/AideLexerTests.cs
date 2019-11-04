using NUnit.Framework;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Tokens;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideLexerTests
    {
        [Test]
        public void Todo()
        {
            // Arrange
            var input = @"CREATE TABLE \(";
            var lexer = new AideLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(3));

            // CREATE
            var token = tokens[0];
            Assert.That(token, Is.TypeOf<WordAideToken>());
            var wordToken = (WordAideToken) token;
            Assert.That(wordToken.Word, Is.EqualTo("CREATE"));
            Assert.That(wordToken.Name, Is.Null);

            // TABLE
            token = tokens[1];
            Assert.That(token, Is.TypeOf<WordAideToken>());
            wordToken = (WordAideToken)token;
            Assert.That(wordToken.Word, Is.EqualTo("TABLE"));
            Assert.That(wordToken.Name, Is.Null);

            // (
            token = tokens[2];
            Assert.That(token, Is.TypeOf<SymbolAideToken>());
            var symbolToken = (SymbolAideToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(AideSymbolValue.LeftParenthesis));
            Assert.That(symbolToken.Name, Is.Null);
        }
    }
}
