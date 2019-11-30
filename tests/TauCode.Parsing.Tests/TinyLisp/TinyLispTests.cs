using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispTests
    {
        [Test]
        public void TodoWat()
        {
            // Arrange

            // Act
            var wat = Symbol.Create("ui");
            var wat2 = Symbol.Create("Ui");

            var kek = Symbol.Create(":10-dd");
            var kek2 = Symbol.Create(":10-Dd");

            var s = Symbol.Create("aaa");
            var sk = Symbol.Create(":aaa");

            // Assert
            var pp = ReferenceEquals(wat, wat2);
            Assert.That(pp);

            Assert.That(wat == wat2);
            Assert.That(ReferenceEquals(wat, wat2));

            Assert.That(kek == kek2);
            Assert.That(ReferenceEquals(kek, kek2));

            var myNil = Symbol.Create("nil");
            var myT = Symbol.Create("t");


            Assert.That(Nil.Instance, Is.SameAs(myNil));
            Assert.That(True.Instance, Is.SameAs(myT));

            Assert.That(s != sk);
        }

        [Test]
        public void TodoWat2()
        {
            // Arrange
            var input =
                @"
; first comment
; second comment";

            // Act
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(2));
            Assert.That(tokens[0] as CommentToken, Has.Property("Comment").EqualTo("; first comment"));
            Assert.That(tokens[1] as CommentToken, Has.Property("Comment").EqualTo("; second comment"));
        }

        [Test]
        public void TodoWat3()
        {
            // Arrange
            var input =
                @"();wat";

            // Act
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(3));

            Assert.That(tokens[0] as PunctuationToken, Has.Property("Value").EqualTo(Punctuation.LeftParenthesis));
            Assert.That(tokens[1] as PunctuationToken, Has.Property("Value").EqualTo(Punctuation.RightParenthesis));
            Assert.That(tokens[2] as CommentToken, Has.Property("Comment").EqualTo(";wat"));
        }

        [Test]
        public void TodoWat4()
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
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(20));

            Assert.That(tokens[0] as CommentToken, Has.Property(nameof(CommentToken.Comment)).EqualTo("; CREATE"));

            Assert.That(
                tokens[1] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[2] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":defblock"));
            Assert.That(tokens[3] as LispSymbolToken, Has.Property(nameof(LispSymbolToken.Symbol)).EqualTo("create"));

            Assert.That(
                tokens[4] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[5] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":word"));

            Assert.That(tokens[6] as StringToken, Has.Property(nameof(StringToken.Value)).EqualTo("CREATE"));

            Assert.That(
                tokens[7] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[8] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[9] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":alt"));

            Assert.That(
                tokens[10] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[11] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":block"));

            Assert.That(
                tokens[12] as LispSymbolToken,
                Has.Property(nameof(LispSymbolToken.Symbol)).EqualTo("create-table"));

            Assert.That(
                tokens[13] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[14] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.LeftParenthesis));

            Assert.That(tokens[15] as KeywordToken, Has.Property(nameof(KeywordToken.Keyword)).EqualTo(":block"));

            Assert.That(
                tokens[16] as LispSymbolToken,
                Has.Property(nameof(LispSymbolToken.Symbol)).EqualTo("create-index"));

            Assert.That(
                tokens[17] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[18] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));

            Assert.That(
                tokens[19] as PunctuationToken,
                Has.Property(nameof(PunctuationToken.Value)).EqualTo(Punctuation.RightParenthesis));
        }

        [Test]
        public void TodoWat5()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);

            // Act
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            // Assert
            // passed
        }

        [Test]
        public void TodoWat6_Reader()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();

            // Act
            var list = reader.Read(tokens);

            // Assert
            Assert.That(list, Has.Count.EqualTo(10));

            var expectedTexts = this.GetType().Assembly
                .GetResourceText("sql-grammar-expected.lisp", true)
                .Split(";;; splitting comment", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            Assert.That(expectedTexts, Has.Count.EqualTo(list.Count()));

            for (int i = 0; i < list.Count; i++)
            {
                var actual = list[i].ToString();

                var alteredActual = actual
                    .Replace(" )", ")")
                    .Replace(" )", ")")
                    .Replace(" (", "(")
                    .Replace(" (", "(");

                var expected = expectedTexts[i]
                    .Replace(Environment.NewLine, " ")
                    .Replace("\t", "")
                    .Replace(" )", ")")
                    .Replace(" )", ")")
                    .Replace(" (", "(")
                    .Replace(" (", "(");

                Assert.That(alteredActual, Is.EqualTo(expected).IgnoreCase);
            }
        }
    }
}
