﻿using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing
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
                new IdentifierToken("my_tab"),
                new SymbolToken('('),
                new IdentifierToken("id"),
                new IdentifierToken("integer"),
                new SymbolToken(','),
                new IdentifierToken("name"),
                new IdentifierToken("text"),
                new SymbolToken(')'),
            };

            var root = this.BuildRoot();

            IParser parser = new Parser();
            var results = parser.Parse(root, tokens);

            var tableInfo = (TableInfo)results[0];
            Assert.That(tableInfo.Name, Is.EqualTo("my_tab"));
            Assert.That(tableInfo.Columns, Has.Count.EqualTo(2));

            var column = tableInfo.Columns[0];
            Assert.That(column.Name, Is.EqualTo("id"));
            Assert.That(column.TypeName, Is.EqualTo("integer"));

            column = tableInfo.Columns[1];
            Assert.That(column.Name, Is.EqualTo("name"));
            Assert.That(column.TypeName, Is.EqualTo("text"));
        }

        private INode BuildRoot()
        {
            INodeFamily family = new NodeFamily("parser_demo");

            var root = new IdleNode(family, "root");
            var create = new ExactWordNode(
                family, 
                "Node: CREATE", 
                null,
                "CREATE");
            var table = new ExactWordNode(
                family, 
                "Node: TABLE", 
                (token, accumulator) =>
                {
                    var tableInfo = new TableInfo();
                    accumulator.AddResult(tableInfo);
                }, 
                "TABLE");
            var tableName = new IdentifierNode(family, "Node: <table_name>", (token, accumulator) =>
            {
                var tableInfo = (TableInfo)accumulator.Last();
                tableInfo.Name = ((IdentifierToken)token).Identifier;
            });
            var leftParen = new ExactSymbolNode(family, "Node: (", null, '(');
            var columnName = new IdentifierNode(family, "Node: <column_name>", (token, accumulator) =>
            {
                var tableInfo = (TableInfo)accumulator.Last();
                var column = new ColumnInfo
                {
                    Name = ((IdentifierToken)token).Identifier,
                };
                tableInfo.Columns.Add(column);
            });
            var typeName = new IdentifierNode(family, "Node: <type_name>", (token, accumulator) =>
            {
                var tableInfo = (TableInfo)accumulator.Last();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((IdentifierToken)token).Identifier;
            });
            var comma = new ExactSymbolNode(family, "Node: ,", null, ',');
            var rightParen = new ExactSymbolNode(family, "Node: )", null, ')');

            root.EstablishLink(create);
            create.EstablishLink(table);
            table.EstablishLink(tableName);
            tableName.EstablishLink(leftParen);
            leftParen.EstablishLink(columnName);
            columnName.EstablishLink(typeName);

            typeName.EstablishLink(comma);
            typeName.EstablishLink(rightParen);

            comma.EstablishLink(columnName);

            rightParen.EstablishLink(EndNode.Instance);

            return root;
        }
    }
}