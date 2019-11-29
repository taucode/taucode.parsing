using NUnit.Framework;
using System;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Data;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

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
            ILexer lexer = new TinyLispLexer
            {
                AddCommentTokens = true,
            };

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
            ILexer lexer = new TinyLispLexer
            {
                AddCommentTokens = true,
            };

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
            ILexer lexer = new TinyLispLexer
            {
                AddCommentTokens = true,
            };

            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(1488));

            Assert.That(tokens[0] as CommentToken, Has.Property("Comment").EqualTo("; CREATE"));
            Assert.That(tokens[1] as PunctuationToken, Has.Property("Value").EqualTo(Punctuation.LeftParenthesis));
            Assert.That(tokens[2] as KeywordToken, Has.Property("Keyword").EqualTo(":defblock"));
            Assert.That(tokens[3] as LispSymbolToken, Has.Property("Symbol").EqualTo("create"));

            throw new NotImplementedException("good, go on!");
        }
    }
}
