﻿using NUnit.Framework;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Sql;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Parsing.Tests.TinyLisp
{
    [TestFixture]
    public class BuilderTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new TinyLispLexer();
        }

        [Test]
        public void Build_EmptyDefblock_ThrowsTinyLispException()
        {
            // Arrange
            var lisp = "(defblock :name foo :is-top t)";
            
            var tokens = _lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            INodeFactory factory = new SqlNodeFactory();

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
            
            var tokens = _lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            INodeFactory factory = new SqlNodeFactory();

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
            
            var tokens = _lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            INodeFactory factory = new SqlNodeFactory();

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
            
            var tokens = _lexer.Lexize(lisp);
            var reader = new TinyLispPseudoReader();
            var pseudoList = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            INodeFactory factory = new SqlNodeFactory();

            // Act
            var ex = Assert.Throws<TinyLispException>(() => builder.Build(factory, pseudoList));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Free arguments not found."));
        }
    }
}
