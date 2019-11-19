using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Building
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void Builder_SqlGrammar_ProducesValidRootNode()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("AllBlocks.txt", true);

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);

            IParser parser = new Parser();
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);

            IBuilder builder = new Builder();

            // Act
            var sqlRoot = builder.Build(results.Cast<BlockDefinitionResult>());

            // Assert

            // CREATE
            var curr = sqlRoot;
            Assert.That(curr, Is.TypeOf<ExactWordNode>());
            Assert.That(curr.Name, Is.Null);
            var wordNode = (ExactWordNode)curr;
            Assert.That(wordNode.Word, Is.EqualTo("CREATE"));

            var nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes.Count, Is.EqualTo(1));

            // TABLE
            curr = nextNodes.Single();
            Assert.That(curr, Is.TypeOf<ExactWordNode>());
            Assert.That(curr.Name, Is.Null);
            wordNode = (ExactWordNode)curr;
            Assert.That(wordNode.Word, Is.EqualTo("TABLE"));

            nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(1));

            // table name
            curr = nextNodes.Single();
            Assert.That(curr, Is.TypeOf<IdentifierNode>());
            Assert.That(curr.Name, Is.EqualTo("table_name"));

            nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(1));
            
            // (
            curr = nextNodes.Single();
            Assert.That(curr, Is.TypeOf<ExactSymbolNode>());
            Assert.That(curr.Name, Is.Null);
            var symbolNode = (ExactSymbolNode)curr;
            Assert.That(symbolNode.Value, Is.EqualTo(SymbolValue.LeftParenthesis));

            nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(1));

            // column_name
            curr = nextNodes.Single();
            Assert.That(curr, Is.TypeOf<IdentifierNode>());
            Assert.That(curr.Name, Is.EqualTo("column_name"));

            nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(1));

            // type_name
            curr = nextNodes.Single();
            Assert.That(curr, Is.TypeOf<IdentifierNode>());
            Assert.That(curr.Name, Is.EqualTo("type_name"));

            nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(4));


            throw new NotImplementedException("go on, looks good!");
        }
    }
}
