﻿using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;
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
            IBuilder builder = new Builder();
            var root = builder.Build(list);
            root.FetchTree();

            // Assert

            // CREATE
            var curr = root;
            Assert.That(curr, Is.TypeOf<ExactWordNode>());
            Assert.That(curr.Name, Is.Null);
            var wordNode = (ExactWordNode)curr;
            Assert.That(wordNode.Word, Is.EqualTo("CREATE"));

            var nextNodes = curr.GetNonIdleLinks();
            Assert.That(nextNodes.Count, Is.EqualTo(3));

            // TABLE
            curr = nextNodes.Single(x => x is ExactWordNode exactWordNode && exactWordNode.Word == "TABLE");
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
            Assert.That(nextNodes, Has.Count.EqualTo(2));

            Assert.That(nextNodes, Does.Contain(tableClosing));

            // CONSTRAINT-s comma
            var constraintsComma = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode4 && symbolNode4.Value == SymbolValue.Comma);

            nextNodes = constraintsComma.GetNonIdleLinks();
            Assert.That(nextNodes.Single(), Is.SameAs(constraint));

            // FOREIGN
            var foreignKey = foreign.GetNonIdleLinks().Single();
            Assert.That(foreignKey, Has.Property("Word").EqualTo("KEY"));

            // fk name
            var fkName = foreignKey.GetNonIdleLinks().Single();
            Assert.That(fkName, Is.TypeOf<IdentifierNode>());
            Assert.That(fkName.Name, Is.EqualTo("fk_name"));

            // fk left paren
            var fkLeftParen = fkName.GetNonIdleLinks().Single();
            Assert.That(fkLeftParen, Is.TypeOf<ExactSymbolNode>());
            symbolNode = (ExactSymbolNode)fkLeftParen;
            Assert.That(symbolNode.Value, Is.EqualTo(SymbolValue.LeftParenthesis));

            // fk column name
            var fkColumnName = fkLeftParen.GetNonIdleLinks().Single();
            Assert.That(fkColumnName, Is.TypeOf<IdentifierNode>());
            Assert.That(fkColumnName.Name, Is.EqualTo("fk_column_name"));

            nextNodes = fkColumnName.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));

            // fk column names comma
            var fkColumnNamesComma = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode5 && symbolNode5.Value == SymbolValue.Comma);
            Assert.That(fkColumnNamesComma.GetNonIdleLinks().Single(), Is.SameAs(fkColumnName));

            // fk column names right paren
            var fkColumnNamesRightParen = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode6 && symbolNode6.Value == SymbolValue.RightParenthesis);

            // REFERENCES
            var references = fkColumnNamesRightParen.GetNonIdleLinks().Single();
            Assert.That(references, Has.Property("Word").EqualTo("REFERENCES"));

            // ref'd table name
            var referencedTableName = references.GetNonIdleLinks().Single();
            Assert.That(referencedTableName, Is.TypeOf<IdentifierNode>());
            Assert.That(referencedTableName.Name, Is.EqualTo("fk_referenced_table_name"));

            // fk ref'd left paren
            var fkReferencedLeftParen = referencedTableName.GetNonIdleLinks().Single();
            Assert.That(fkReferencedLeftParen, Has.Property("Value").EqualTo(SymbolValue.LeftParenthesis));

            // fk ref'd column name
            var fkReferencedColumnName = fkReferencedLeftParen.GetNonIdleLinks().Single();
            Assert.That(fkReferencedColumnName, Is.TypeOf<IdentifierNode>());
            Assert.That(fkReferencedColumnName.Name, Is.EqualTo("fk_referenced_column_name"));

            nextNodes = fkReferencedColumnName.GetNonIdleLinks();

            // fk ref'd column name comma
            var fkReferencedColumnNameComma = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode7 && symbolNode7.Value == SymbolValue.Comma);
            Assert.That(fkReferencedColumnNameComma.GetNonIdleLinks().Single(), Is.SameAs(fkReferencedColumnName));

            // fk ref'd columns right paren
            var fkReferencedColumnsRightParen = nextNodes.Single(x =>
                x is ExactSymbolNode symbolNode8 && symbolNode8.Value == SymbolValue.RightParenthesis);

            nextNodes = fkReferencedColumnsRightParen.GetNonIdleLinks();
            Assert.That(nextNodes, Has.Count.EqualTo(2));

            Assert.That(nextNodes, Does.Contain(tableClosing));
            Assert.That(nextNodes, Does.Contain(constraintsComma));
        }
    }
}