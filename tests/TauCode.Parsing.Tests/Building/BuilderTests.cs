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
            var columnName = curr;
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

            // comma
            var comma = nextNodes.Single(x => x.Name == "comma");

            // table_closing
            var tableClosing = nextNodes.Single(x => x.Name == "table_closing");

            // NULL
            var nullWord = nextNodes.Single(x => x.Name == "null");

            // NOT
            var not = nextNodes.Single(x => x is ExactWordNode wordNode2 && wordNode2.Word == "NOT");

            // inspect NULL
            nextNodes = nullWord.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));

            Assert.That(nextNodes, Does.Contain(comma));
            Assert.That(nextNodes, Does.Contain(tableClosing));

            // inspect NOT_NULL
            var notNull = not.GetNonIdleLinks().Single();
            wordNode = (ExactWordNode)notNull;
            Assert.That(wordNode.Word, Is.EqualTo("NULL"));
            Assert.That(wordNode.Name, Is.EqualTo("not_null"));

            nextNodes = notNull.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));

            Assert.That(nextNodes, Does.Contain(comma));
            Assert.That(nextNodes, Does.Contain(tableClosing));

            // deal with comma
            Assert.That(comma, Is.TypeOf<ExactSymbolNode>());
            symbolNode = (ExactSymbolNode)comma;
            Assert.That(symbolNode.Value, Is.EqualTo(SymbolValue.Comma));

            nextNodes = comma.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));

            Assert.That(nextNodes, Does.Contain(columnName));
            var constraint = nextNodes.Single(x => x is ExactWordNode wordNode3 && wordNode3.Word == "CONSTRAINT");

            // CONSTRAINT
            Assert.That(constraint.Name, Is.EqualTo("begin"));
            nextNodes = constraint.GetNonIdleLinks();

            var primary = nextNodes.Single(x => ((dynamic)x).Word == "PRIMARY");
            var foreign = nextNodes.Single(x => ((dynamic)x).Word == "FOREIGN");

            // PRIMARY
            nextNodes = primary.GetNonIdleLinks();
            
            // (primary)_KEY
            var primaryKey = nextNodes.Single();
            Assert.That(primaryKey, Has.Property("Word").EqualTo("KEY"));

            // pk_name
            var pkName = primaryKey.GetNonIdleLinks().Single();
            Assert.That(pkName.Name, Is.EqualTo("pk_name"));
            Assert.That(pkName, Is.TypeOf<IdentifierNode>());

            // primary key (
            var pkLeftParent = pkName.GetNonIdleLinks().Single();
            Assert.That(pkLeftParent, Has.Property("Value").EqualTo(SymbolValue.LeftParenthesis));

            // pk_column_name
            var pkColumnName = pkLeftParent.GetNonIdleLinks().Single();
            Assert.That(pkColumnName, Is.TypeOf<IdentifierNode>());
            Assert.That(pkColumnName.Name, Is.EqualTo("pk_column_name"));

            nextNodes = pkColumnName.GetNonIdleLinks();

            Assert.That(nextNodes, Has.Count.EqualTo(4));

            var asc = nextNodes.Single(x => x is ExactWordNode wordNode4 && wordNode4.Word == "ASC");
            var desc = nextNodes.Single(x => x is ExactWordNode wordNode5 && wordNode5.Word == "DESC");
            var pkColumnComma = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode2 && symbolNode2.Value == SymbolValue.Comma);
            var pkColumnsRightParen = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode3 && symbolNode3.Value == SymbolValue.RightParenthesis);

            // ASC
            Assert.That(asc.Name, Is.EqualTo("asc"));
            nextNodes = asc.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));
            Assert.That(nextNodes, Does.Contain(pkColumnComma));
            Assert.That(nextNodes, Does.Contain(pkColumnsRightParen));

            // ASC
            Assert.That(desc.Name, Is.EqualTo("desc"));
            nextNodes = desc.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));
            Assert.That(nextNodes, Does.Contain(pkColumnComma));
            Assert.That(nextNodes, Does.Contain(pkColumnsRightParen));

            // pk columns comma
            nextNodes = pkColumnComma.GetNonIdleLinks();
            Assert.That(nextNodes.Single(), Is.SameAs(pkColumnName));

            // pk columns right paren
            nextNodes = pkColumnsRightParen.GetNonIdleLinks();

            Assert.That(nextNodes, Does.Contain(tableClosing));



            throw new NotImplementedException("go on, looks good!");
        }
    }
}
