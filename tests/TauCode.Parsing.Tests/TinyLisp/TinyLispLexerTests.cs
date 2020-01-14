using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispLexerTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new /*Tiny-LispLexer()*/ OmicronTinyLispLexer();
        }

        [Test]
        public void Lexize_OnlyComments_EmptyOutput()
        {
            // Arrange
            var input =
                @"
; first comment
; second comment";

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(0)); // comments will not be added as tokens.
        }

        [Test]
        public void Lexize_HasComments_OmitsComments()
        {
            // Arrange
            var input =
@"();wat
-1599 -1599-";

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(4));

            var punctuationToken = (LispPunctuationToken)tokens[0];
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(0, 0)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));

            punctuationToken = (LispPunctuationToken)tokens[1];
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(0, 1)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));

            var integerToken = (IntegerToken)tokens[2];
            Assert.That(integerToken.Position, Is.EqualTo(new Position(1, 0)));
            Assert.That(integerToken.ConsumedLength, Is.EqualTo(5));
            Assert.That(integerToken.Value, Is.EqualTo("-1599"));

            var symbolToken = (LispSymbolToken)tokens[3];
            Assert.That(symbolToken.Position, Is.EqualTo(new Position(1, 6)));
            Assert.That(symbolToken.ConsumedLength, Is.EqualTo(6));
            Assert.That(symbolToken.SymbolName, Is.EqualTo("-1599-"));
        }

        [Test]
        public void Lexize_ComplexInput_ProducesValidResult()
        {
            // Arrange
            var input =
@"
; CREATE
(:defblock create
    (:word ""CREATE"")
    (:alt (:block create-table) (:block create-index))
)
";

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(19));

            //  0: (
            var punctuationToken = (LispPunctuationToken)tokens[0];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(2, 0)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            //  1: :defblock
            var keywordToken = (KeywordToken)tokens[1];
            Assert.That(keywordToken.Keyword, Is.EqualTo(":defblock"));
            Assert.That(keywordToken.Position, Is.EqualTo(new Position(2, 1)));
            Assert.That(keywordToken.ConsumedLength, Is.EqualTo(9));

            //  2: create
            var symbolToken = (LispSymbolToken)tokens[2];
            Assert.That(symbolToken.SymbolName, Is.EqualTo("create"));
            Assert.That(symbolToken.Position, Is.EqualTo(new Position(2, 11)));
            Assert.That(symbolToken.ConsumedLength, Is.EqualTo(6));

            //  3: (
            punctuationToken = (LispPunctuationToken)tokens[3];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(3, 4)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            //  4: :word
            keywordToken = (KeywordToken)tokens[4];
            Assert.That(keywordToken.Keyword, Is.EqualTo(":word"));
            Assert.That(keywordToken.Position, Is.EqualTo(new Position(3, 5)));
            Assert.That(keywordToken.ConsumedLength, Is.EqualTo(5));

            //  5: "CREATE"
            var textToken = (TextToken)tokens[5];
            Assert.That(textToken.Text, Is.EqualTo("CREATE"));
            Assert.That(textToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(textToken.Decoration, Is.SameAs(DoubleQuoteTextDecoration.Instance));
            Assert.That(textToken.Position, Is.EqualTo(new Position(3, 11)));
            Assert.That(textToken.ConsumedLength, Is.EqualTo(8));

            //  6: )
            punctuationToken = (LispPunctuationToken)tokens[6];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(3, 19)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            //  7: (
            punctuationToken = (LispPunctuationToken)tokens[7];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(4, 4)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            //  8: :alt
            keywordToken = (KeywordToken)tokens[8];
            Assert.That(keywordToken.Keyword, Is.EqualTo(":alt"));
            Assert.That(keywordToken.Position, Is.EqualTo(new Position(4, 5)));
            Assert.That(keywordToken.ConsumedLength, Is.EqualTo(4));

            //  9: (
            punctuationToken = (LispPunctuationToken)tokens[9];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(4, 10)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            // 10: :block
            keywordToken = (KeywordToken)tokens[10];
            Assert.That(keywordToken.Keyword, Is.EqualTo(":block"));
            Assert.That(keywordToken.Position, Is.EqualTo(new Position(4, 11)));
            Assert.That(keywordToken.ConsumedLength, Is.EqualTo(6));

            // 11: create-table
            symbolToken = (LispSymbolToken)tokens[11];
            Assert.That(symbolToken.SymbolName, Is.EqualTo("create-table"));
            Assert.That(symbolToken.Position, Is.EqualTo(new Position(4, 18)));
            Assert.That(symbolToken.ConsumedLength, Is.EqualTo(12));

            // 12: )
            punctuationToken = (LispPunctuationToken)tokens[12];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(4, 30)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            // 13: (
            punctuationToken = (LispPunctuationToken)tokens[13];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.LeftParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(4, 32)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            // 14: :block
            keywordToken = (KeywordToken)tokens[14];
            Assert.That(keywordToken.Keyword, Is.EqualTo(":block"));
            Assert.That(keywordToken.Position, Is.EqualTo(new Position(4, 33)));
            Assert.That(keywordToken.ConsumedLength, Is.EqualTo(6));

            // 15: create-index
            symbolToken = (LispSymbolToken)tokens[15];
            Assert.That(symbolToken.SymbolName, Is.EqualTo("create-index"));
            Assert.That(symbolToken.Position, Is.EqualTo(new Position(4, 40)));
            Assert.That(symbolToken.ConsumedLength, Is.EqualTo(12));

            // 16: )
            punctuationToken = (LispPunctuationToken)tokens[16];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(4, 52)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            // 17: )
            punctuationToken = (LispPunctuationToken)tokens[17];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(4, 53)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));

            // 18: )
            punctuationToken = (LispPunctuationToken)tokens[18];
            Assert.That(punctuationToken.Value, Is.EqualTo(Punctuation.RightParenthesis));
            Assert.That(punctuationToken.Position, Is.EqualTo(new Position(5, 0)));
            Assert.That(punctuationToken.ConsumedLength, Is.EqualTo(1));
        }

        [Test]
        public void Lexize_SqlGrammar_LexizesCorrectly()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            // passed
        }

        [Test]
        public void Lexize_SimpleForm_LexizesCorrectly()
        {
            // Arrange
            var input = "(a . b)";

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(5));
            Assert.That(tokens[0] as LispPunctuationToken, Has.Property(nameof(EnumToken<Punctuation>.Value)).EqualTo(Punctuation.LeftParenthesis));
            Assert.That(tokens[1] as LispSymbolToken, Has.Property(nameof(LispSymbolToken.SymbolName)).EqualTo("a"));
            Assert.That(tokens[2] as LispPunctuationToken, Has.Property(nameof(EnumToken<Punctuation>.Value)).EqualTo(Punctuation.Period));
            Assert.That(tokens[3] as LispSymbolToken, Has.Property(nameof(LispSymbolToken.SymbolName)).EqualTo("b"));
            Assert.That(tokens[4] as LispPunctuationToken, Has.Property(nameof(EnumToken<Punctuation>.Value)).EqualTo(Punctuation.RightParenthesis));
        }

        [Test]
        [TestCase("\r \n \r\n \n\r   \"not close", 5, 13, Description = "Not closed string 'not close'")]
        public void Lexize_UnexpectedEnd_ThrowsLexerException(string notClosedString, int line, int column)
        {
            // Arrange
            var input = notClosedString;

            // Act
            var ex = Assert.Throws<LexingException>(() => _lexer.Lexize(input));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unclosed string."));
            Assert.That(ex.Position, Is.EqualTo(new Position(line, column)));
        }

        [Test]
        [TestCase("\n \r\"broken\ncontinue on new line\"", "broken\ncontinue on new line")]
        [TestCase("\n \r \"broken\rcontinue on new line\"", "broken\rcontinue on new line")]
        [TestCase("\n \r  \"broken\r\ncontinue on new line\"", "broken\r\ncontinue on new line")]
        public void Lexize_NewLineInString_LexizesCorrectly(
            string notClosedString,
            string expectedExtractedString)
        {
            // Arrange
            var input = notClosedString;

            // Act
            var tokens = _lexer.Lexize(input);
            var token = (TextToken)tokens.Single();

            // Assert
            Assert.That(token.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(token.Decoration, Is.SameAs(DoubleQuoteTextDecoration.Instance));
            Assert.That(token.Text, Is.EqualTo(expectedExtractedString));
        }

        [Test]
        [TestCase("symbol at end", typeof(LispSymbolToken))]
        [TestCase("keyword at :end", typeof(KeywordToken))]
        [TestCase("integer at end 1488", typeof(IntegerToken))]
        [TestCase("string at \"end\"", typeof(TextToken))]
        [TestCase("( punctuation at end )", typeof(LispPunctuationToken))]
        [TestCase("comment :somma ;end", typeof(KeywordToken))]
        public void Lexize_TokenAtEnd_LexizedCorrectly(string input, Type lastTokenExpectedType)
        {
            // Arrange

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens.Last(), Is.TypeOf(lastTokenExpectedType));
        }

        [Test]
        public void Lexize_IntAndTheLike_LexizedCorrectly()
        {
            // Arrange
            var input1 = "1";
            var input2 = "+1";
            var input3 = "-1";
            var input4 = "-133";
            var input5 = "+391";
            var input6 = "+";
            var input7 = "-";
            var input8 = "1-";

            // Act
            var token1 = _lexer.Lexize(input1).Single();
            var token2 = _lexer.Lexize(input2).Single();
            var token3 = _lexer.Lexize(input3).Single();
            var token4 = _lexer.Lexize(input4).Single();
            var token5 = _lexer.Lexize(input5).Single();
            var token6 = _lexer.Lexize(input6).Single();
            var token7 = _lexer.Lexize(input7).Single();
            var token8 = _lexer.Lexize(input8).Single();

            // Assert
            Assert.That(token1 as IntegerToken, Has.Property("Value").EqualTo("1"));
            Assert.That(token2 as IntegerToken, Has.Property("Value").EqualTo("1"));
            Assert.That(token3 as IntegerToken, Has.Property("Value").EqualTo("-1"));
            Assert.That(token4 as IntegerToken, Has.Property("Value").EqualTo("-133"));
            Assert.That(token5 as IntegerToken, Has.Property("Value").EqualTo("391"));
            Assert.That(token6 as LispSymbolToken, Has.Property("SymbolName").EqualTo("+"));
            Assert.That(token7 as LispSymbolToken, Has.Property("SymbolName").EqualTo("-"));
            Assert.That(token8 as LispSymbolToken, Has.Property("SymbolName").EqualTo("1-"));
        }

        [Test]
        public void Lexize_CrAtInputEnd_LexizedCorrectly()
        {
            // Arrange
            var input = "a\r";

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            var token = (LispSymbolToken)tokens.Single();
            Assert.That(token.SymbolName, Is.EqualTo("a"));
        }

        [Test]
        [TestCase(" ;comment\r")]
        [TestCase(" ;comment\r ")]
        [TestCase(" ;comment\n")]
        [TestCase(" ;comment\n ")]
        [TestCase(" ;comment\r\n")]
        [TestCase(" ;comment\n\r")]
        public void Lexize_CommentWithLineEndings_LexizedCorrectly(string input)
        {
            // Arrange

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Is.Empty);
        }

        [Test]
        [TestCase("\r\n\"abc\n def\" \"mno \r\n \"  \"lek\r guk\" \"zz\"")]
        public void Lexize_NewLineInString_PositionIsCorrect(string input)
        {
            // Arrange
            ILexer lexer = new /*Tiny-LispLexer()*/ OmicronTinyLispLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens.Count, Is.EqualTo(4));
            Assert.That(tokens.All(x => x is TextToken), Is.True);

            var textTokens = tokens.Select(x => (TextToken)x).ToList();

            var textToken = textTokens[0];
            Assert.That(textToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(textToken.Decoration, Is.SameAs(DoubleQuoteTextDecoration.Instance));
            Assert.That(textToken.Text, Is.EqualTo("abc\n def"));
            Assert.That(textToken.Position, Is.EqualTo(new Position(1, 0)));
            Assert.That(textToken.ConsumedLength, Is.EqualTo(10));

            textToken = textTokens[1];
            Assert.That(textToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(textToken.Decoration, Is.SameAs(DoubleQuoteTextDecoration.Instance));
            Assert.That(textToken.Text, Is.EqualTo("mno \r\n "));
            Assert.That(textToken.Position, Is.EqualTo(new Position(2, 6)));
            Assert.That(textToken.ConsumedLength, Is.EqualTo(9));

            textToken = textTokens[2];
            Assert.That(textToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(textToken.Decoration, Is.SameAs(DoubleQuoteTextDecoration.Instance));
            Assert.That(textToken.Text, Is.EqualTo("lek\r guk"));
            Assert.That(textToken.Position, Is.EqualTo(new Position(3, 4)));
            Assert.That(textToken.ConsumedLength, Is.EqualTo(10));

            textToken = textTokens[3];
            Assert.That(textToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(textToken.Decoration, Is.SameAs(DoubleQuoteTextDecoration.Instance));
            Assert.That(textToken.Text, Is.EqualTo("zz"));
            Assert.That(textToken.Position, Is.EqualTo(new Position(4, 6)));
            Assert.That(textToken.ConsumedLength, Is.EqualTo(4));
        }
    }
}
