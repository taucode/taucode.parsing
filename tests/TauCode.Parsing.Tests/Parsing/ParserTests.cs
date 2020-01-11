using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing
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
            INode exactText = new ExactTextNode(
                "foo",
                new[] { WordTextClass.Instance, },
                false,
                (node, token, arg3) => { },
                nodeFamily,
                null);
            INode someText = new TextNode(
                new ITextClass[] { WordTextClass.Instance, },
                null,
                nodeFamily,
                null);
            idle.EstablishLink(someText);
            idle.EstablishLink(exactText);
            someText.EstablishLink(EndNode.Instance);
            exactText.EstablishLink(EndNode.Instance);

            IParserLab parser = new ParserLab();

            var tokens = new List<IToken>
            {
                new TextToken(
                    WordTextClass.Instance,
                    NoneTextDecoration.Instance, 
                    "foo",
                    Position.Zero, 
                    3),
            };

            // Act
            parser.Root = idle;
            var ex = Assert.Throws<NodeConcurrencyException>(() => parser.Parse(tokens));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("More than one node accepted the token."));
            Assert.That(ex.ConcurrentNodes, Has.Length.EqualTo(2));
            Assert.That(ex.ConcurrentNodes, Does.Contain(exactText));
            Assert.That(ex.ConcurrentNodes, Does.Contain(someText));
            Assert.That(ex.Token, Is.SameAs(tokens.Single()));
        }
    }
}
