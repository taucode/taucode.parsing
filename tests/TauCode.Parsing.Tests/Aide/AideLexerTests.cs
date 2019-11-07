using NUnit.Framework;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideLexerTests
    {
        [Test]
        public void Todo()
        {
            // Arrange
            var input =
@"
CREATE TABLE <table_name>\Identifier \(
    <column_definition>\Block <comma>\,
    <constraint_definitions>\Block
<table_closing>\) \End
";
            var lexer = new AideLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(9));

            // CREATE
            var token = tokens[0];
            Assert.That(token, Is.TypeOf<WordAideToken>());
            var wordToken = (WordAideToken)token;
            Assert.That(wordToken.Word, Is.EqualTo("CREATE"));
            Assert.That(wordToken.Name, Is.Null);

            // TABLE
            token = tokens[1];
            Assert.That(token, Is.TypeOf<WordAideToken>());
            wordToken = (WordAideToken)token;
            Assert.That(wordToken.Word, Is.EqualTo("TABLE"));
            Assert.That(wordToken.Name, Is.Null);

            // table name
            token = tokens[2];
            Assert.That(token, Is.TypeOf<SyntaxElementAideToken>());
            var syntaxElementToken = (SyntaxElementAideToken)token;
            Assert.That(syntaxElementToken.SyntaxElement, Is.EqualTo(SyntaxElement.Identifier));
            Assert.That(syntaxElementToken.Name, Is.EqualTo("table_name"));

            // (
            token = tokens[3];
            Assert.That(token, Is.TypeOf<SymbolAideToken>());
            var symbolToken = (SymbolAideToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(SymbolValue.LeftParenthesis));
            Assert.That(symbolToken.Name, Is.Null);

            // column definition
            token = tokens[4];
            Assert.That(token, Is.TypeOf<SyntaxElementAideToken>());
            syntaxElementToken = (SyntaxElementAideToken)token;
            Assert.That(syntaxElementToken.SyntaxElement, Is.EqualTo(SyntaxElement.Block));
            Assert.That(syntaxElementToken.Name, Is.EqualTo("column_definition"));

            // ,
            token = tokens[5];
            Assert.That(token, Is.TypeOf<SymbolAideToken>());
            symbolToken = (SymbolAideToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(SymbolValue.Comma));
            Assert.That(symbolToken.Name, Is.EqualTo("comma"));

            // constraint definitions
            token = tokens[6];
            Assert.That(token, Is.TypeOf<SyntaxElementAideToken>());
            syntaxElementToken = (SyntaxElementAideToken)token;
            Assert.That(syntaxElementToken.SyntaxElement, Is.EqualTo(SyntaxElement.Block));
            Assert.That(syntaxElementToken.Name, Is.EqualTo("constraint_definitions"));

            // )
            token = tokens[7];
            Assert.That(token, Is.TypeOf<SymbolAideToken>());
            symbolToken = (SymbolAideToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(SymbolValue.RightParenthesis));
            Assert.That(symbolToken.Name, Is.EqualTo("table_closing"));

            // end
            token = tokens[8];
            Assert.That(token, Is.TypeOf<SyntaxElementAideToken>());
            syntaxElementToken = (SyntaxElementAideToken)token;
            Assert.That(syntaxElementToken.SyntaxElement, Is.EqualTo(SyntaxElement.End));
            Assert.That(syntaxElementToken.Name, Is.Null);
        }
    }
}
