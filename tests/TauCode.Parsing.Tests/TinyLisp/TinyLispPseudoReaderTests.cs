using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class TinyLispPseudoReaderTests
    {
        [Test]
        public void Read_SqlGrammar_ProducesExpectedResult()
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

        [Test]
        public void Read_UnclosedForm_ThrowsTinyLispException()
        {
            // Arrange
            var form = "(unclosed (a (bit))";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unclosed form."));
        }

        [Test]
        public void Read_ExtraRightParenthesis_ThrowsTinyLispException()
        {
            // Arrange
            var form = "(closed too much))";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(form);
            var reader = new TinyLispPseudoReader();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Unexpected ')'."));
        }

        [Test]
        public void Read_UnsupportedToken_ThrowsTinyLispException()
        {
            // Arrange
            var form = "(some good form)";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(form);
            var badToken = new EnumToken<int>(1488);
            tokens.Insert(1, badToken);
            var reader = new TinyLispPseudoReader();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => reader.Read(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"Could not read token of type '{badToken.GetType().FullName}'."));
        }
    }
}
