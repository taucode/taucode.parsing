﻿using NUnit.Framework;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideLexerTests
    {
        [Test]
        public void Lexize_ValidInput_Lexizes()
        {
            // Arrange
            var input =
@"
CREATE TABLE <table_name>\Identifier \(
    <column_definition>\BlockReference <comma>\,
    <constraint_definitions>\BlockReference
<table_closing>\)
";
            var lexer = new AideLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(8));

            // CREATE
            var token = tokens[0];
            Assert.That(token, Is.TypeOf<WordToken>());
            var wordToken = (WordToken)token;
            Assert.That(wordToken.Word, Is.EqualTo("CREATE"));
            Assert.That(wordToken.Name, Is.Null);

            // TABLE
            token = tokens[1];
            Assert.That(token, Is.TypeOf<WordToken>());
            wordToken = (WordToken)token;
            Assert.That(wordToken.Word, Is.EqualTo("TABLE"));
            Assert.That(wordToken.Name, Is.Null);

            // table name
            token = tokens[2];
            Assert.That(token, Is.TypeOf<EnumToken<SyntaxElement>>());
            var syntaxElementToken = (EnumToken<SyntaxElement>)token;
            Assert.That(syntaxElementToken.Value, Is.EqualTo(SyntaxElement.Identifier));
            Assert.That(syntaxElementToken.Name, Is.EqualTo("table_name"));

            // (
            token = tokens[3];
            Assert.That(token, Is.TypeOf<SymbolToken>());
            var symbolToken = (SymbolToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(SymbolValue.LeftParenthesis));
            Assert.That(symbolToken.Name, Is.Null);

            // column definition
            token = tokens[4];
            Assert.That(token, Is.TypeOf<EnumToken<SyntaxElement>>());
            syntaxElementToken = (EnumToken<SyntaxElement>)token;
            Assert.That(syntaxElementToken.Value, Is.EqualTo(SyntaxElement.BlockReference));
            Assert.That(syntaxElementToken.Name, Is.EqualTo("column_definition"));

            // ,
            token = tokens[5];
            Assert.That(token, Is.TypeOf<SymbolToken>());
            symbolToken = (SymbolToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(SymbolValue.Comma));
            Assert.That(symbolToken.Name, Is.EqualTo("comma"));

            // constraint definitions
            token = tokens[6];
            Assert.That(token, Is.TypeOf<EnumToken<SyntaxElement>>());
            syntaxElementToken = (EnumToken<SyntaxElement>)token;
            Assert.That(syntaxElementToken.Value, Is.EqualTo(SyntaxElement.BlockReference));
            Assert.That(syntaxElementToken.Name, Is.EqualTo("constraint_definitions"));

            // )
            token = tokens[7];
            Assert.That(token, Is.TypeOf<SymbolToken>());
            symbolToken = (SymbolToken)token;
            Assert.That(symbolToken.Value, Is.EqualTo(SymbolValue.RightParenthesis));
            Assert.That(symbolToken.Name, Is.EqualTo("table_closing"));
        }
    }
}
