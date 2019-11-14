using NUnit.Framework;
using System;
using TauCode.Parsing.Nodes2;
using TauCode.Parsing.Tests.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void Parse_ValidInput_Parses()
        {
            // Arrange
            var tokens = new IToken[]
            {
                new WordToken("CREATE"),
                new WordToken("TABLE"),
                new WordToken("my_tab"),
                new SymbolToken('('),
                new WordToken("id"),
                new WordToken("integer"),
                new SymbolToken(','),
                new WordToken("name"),
                new WordToken("text"),
                new SymbolToken(')'),
            };

            var root = this.BuildRoot();

            IParser2 parser = new Parser2();
            var results = parser.Parse(root, tokens);

            var tableInfo = (TableInfo)results[0];
            Assert.That(tableInfo.Name, Is.EqualTo("my_tab"));
            Assert.That(tableInfo.Columns, Has.Count.EqualTo(2));

            var column = tableInfo.Columns[0];
            Assert.That(column.ColumnName, Is.EqualTo("id"));
            Assert.That(column.TypeName, Is.EqualTo("integer"));

            column = tableInfo.Columns[1];
            Assert.That(column.ColumnName, Is.EqualTo("name"));
            Assert.That(column.TypeName, Is.EqualTo("text"));

            //IParser parser = new Parser();

            //// Act
            //var result = parser.Parse(tokens);

            //// Assert
            //var table = result.GetLastResult<dynamic>();

            //var name = (string)table.Name;
            //var columns = (List<dynamic>)table.Columns;

            //Assert.That(name, Is.EqualTo("my_tab"));

            //Assert.That(columns, Has.Count.EqualTo(2));

            //var column = columns[0];
            //Assert.That(column.Name, Is.EqualTo("id"));
            //Assert.That(column.Type, Is.EqualTo("integer"));

            //column = columns[1];
            //Assert.That(column.Name, Is.EqualTo("name"));
            //Assert.That(column.Type, Is.EqualTo("text"));
        }

        private INode2 BuildRoot()
        {
            INodeFamily family = new NodeFamily();

            var root = new IdleNode(family, "root");
            var create = new ExactWordNode(family, "Node: CREATE", "CREATE", null);
            var table = new ExactWordNode(family, "Node: TABLE", "TABLE", (token, accumulator) =>
            {
                throw new NotImplementedException();
            });
            var tableName = new IdentifierNode(family, "Node: <table_name>", (token, accumulator) =>
            {
                throw new NotImplementedException();
            });
            var leftParen = new ExactSymbolNode(family, "Node: (", null, '(');
            var columnName = new IdentifierNode(family, "Node: <column_name>", (token, accumulator) =>
            {
                throw new NotImplementedException();
            });
            var typeName = new IdentifierNode(family, "Node: <type_name>", (token, accumulator) =>
            {
                throw new NotImplementedException();
            });
            var comma = new ExactSymbolNode(
                family,
                "Node: ,",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                ',');
            var rightParen = new ExactSymbolNode(
                family,
                "Node: (",
                (token, accumulator) =>
                {
                    throw new NotImplementedException();
                },
                '(');

            root.AddLink(create);
            create.AddLink(table);
            table.AddLink(tableName);
            tableName.AddLink(leftParen);
            leftParen.AddLink(columnName);
            columnName.AddLink(typeName);

            typeName.AddLink(comma);
            typeName.AddLink(rightParen);

            comma.AddLink(columnName);

            rightParen.AddLink(EndNode.Instance);

            return root;
        }
    }
}
