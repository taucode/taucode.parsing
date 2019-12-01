using NUnit.Framework;
using System;
using TauCode.Parsing.Aide2;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Building2
{
    [TestFixture]
    public class Builder2Tests
    {
        [Test]
        public void TodoWatBuild2()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);
            ILexer lexer = new TinyLispLexer();

            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);

            // Act
            IBuilder2 builder = new Builder2();
            var root = builder.Build(list);
            root.FetchTree();

            // Assert
            Assert.That(root, Is.TypeOf<ExactWordNode>());
            var exactWord = (ExactWordNode) root;
            Assert.That(exactWord.Word, Is.EqualTo("CREATE"));

            throw new NotImplementedException("good, go on!");
        }
    }
}
