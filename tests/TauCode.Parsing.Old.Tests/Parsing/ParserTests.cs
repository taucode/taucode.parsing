using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Old.Nodes;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.Tests.Parsing
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parse_Concurrency_ThrowsNodeConcurrencyException()
        {
            // Arrange
            INodeFamily nodeFamily = new NodeFamily("family");
            INode idle = new IdleNode(nodeFamily, null);
            INode exactText = new OldExactTextNode(
                "foo",
                new[] { OldWordTextClass.Instance, },
                (node, token, arg3) => { },
                nodeFamily,
                null);
            INode someText = new OldTextNode(
                new IOldTextClass[] { OldWordTextClass.Instance, },
                null,
                nodeFamily,
                null);
            idle.EstablishLink(someText);
            idle.EstablishLink(exactText);
            someText.EstablishLink(EndNode.Instance);
            exactText.EstablishLink(EndNode.Instance);

            IOldParser parser = new OldParser();

            var tokens = new List<IToken>
            {
                new OldTextToken(
                    OldWordTextClass.Instance,
                    OldNoneTextDecoration.Instance, 
                    "foo",
                    Position.Zero, 
                    3),
            };

            // Act
            var ex = Assert.Throws<NodeConcurrencyException>(() => parser.ParseOld(idle, tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("More than one node accepted the token."));
            Assert.That(ex.ConcurrentNodes, Has.Length.EqualTo(2));
            Assert.That(ex.ConcurrentNodes, Does.Contain(exactText));
            Assert.That(ex.ConcurrentNodes, Does.Contain(someText));
            Assert.That(ex.Token, Is.SameAs(tokens.Single()));
        }
    }
}
