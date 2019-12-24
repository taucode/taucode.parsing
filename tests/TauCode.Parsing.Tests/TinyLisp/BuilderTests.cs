using NUnit.Framework;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing;
using TauCode.Parsing.Tests.Parsing.Sql;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void Build_EmptyDefblock_ThrowsTinyLispException()
        {
            // Arrange
            var lisp = "(defblock :name foo :is-top t)";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            IBuilder builder = new Builder();
            INodeFactory factory = new SqlNodeFactory("foo");

            // Act
            var ex = Assert.Throws<TinyLispException>(() => builder.Build(factory, pseudoList));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Free arguments not found."));
        }

        [Test]
        public void Build_EmptyOpt_ThrowsTinyLispException()
        {
            // Arrange
            var lisp = "(defblock :name foo :is-top t (opt))";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            IBuilder builder = new Builder();
            INodeFactory factory = new SqlNodeFactory("foo");

            // Act
            var ex = Assert.Throws<TinyLispException>(() => builder.Build(factory, pseudoList));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Free arguments not found."));
        }

        [Test]
        public void Build_EmptyAlt_ThrowsTinyLispException()
        {
            // Arrange
            var lisp = "(defblock :name foo :is-top t (alt))";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            IBuilder builder = new Builder();
            INodeFactory factory = new SqlNodeFactory("foo");

            // Act
            var ex = Assert.Throws<TinyLispException>(() => builder.Build(factory, pseudoList));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Free arguments not found."));
        }

        [Test]
        public void Build_EmptySeq_ThrowsTinyLispException()
        {
            // Arrange
            var lisp = "(defblock :name foo :is-top t (seq))";
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            IBuilder builder = new Builder();
            INodeFactory factory = new SqlNodeFactory("foo");

            // Act
            var ex = Assert.Throws<TinyLispException>(() => builder.Build(factory, pseudoList));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Free arguments not found."));
        }
    }
}
