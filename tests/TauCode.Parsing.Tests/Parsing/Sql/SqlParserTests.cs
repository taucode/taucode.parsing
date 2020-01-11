using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Sql.Data;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    [TestFixture]
    public class SqlParserTests
    {
        private ILexer _tinyLispLexer;
        private ILexer _sqlLexer;

        [SetUp]
        public void SetUp()
        {
            _tinyLispLexer = new TinyLispLexerLab();
            _sqlLexer = new SqlLexer();
        }

        [Test]
        public void SqlParser_ValidInput_Parses()
        {
            // Arrange
            var nodeFactory = new SqlNodeFactory();
            var input = this.GetType().Assembly.GetResourceText("sql-grammar.lisp", true);

            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new TinyLispPseudoReaderLab();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var root = builder.Build(nodeFactory, list);

            IParserLab parser = new ParserLab();

            var allSqlNodes = root.FetchTree();

            var exactTextNodes = allSqlNodes
                .Where(x => x is ExactTextNode)
                .Cast<ExactTextNode>()
                .ToList();

            // todo clean
            //foreach (var exactTextNode in exactTextNodes)
            //{
            //    exactTextNode.IsCaseSensitive = false;
            //}

            //var reservedWords = exactTextNodes
            //    .Select(x => x.ExactText)
            //    .Distinct()
            //    .Select(x => x.ToUpperInvariant())
            //    .ToHashSet();

            #region assign job to nodes

            // table
            var createTable = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "do-create-table", StringComparison.InvariantCultureIgnoreCase));
            createTable.Action = (node, token, accumulator) =>
            {
                var tableInfo = new TableInfo();
                accumulator.AddResult(tableInfo);
            };

            var tableName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "table-name", StringComparison.InvariantCultureIgnoreCase));
            tableName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.Name = ((TextToken)token).Text;
            };

            var columnName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "column-name", StringComparison.InvariantCultureIgnoreCase));
            columnName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = new ColumnInfo
                {
                    Name = ((TextToken)token).Text,
                };
                tableInfo.Columns.Add(columnInfo);
            };

            var typeName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "type-name", StringComparison.InvariantCultureIgnoreCase));
            typeName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((TextToken)token).Text;
            };

            var precision = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "precision", StringComparison.InvariantCultureIgnoreCase));
            precision.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Precision = ((IntegerToken)token).Value.ToInt32();
            };

            var scale = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "scale", StringComparison.InvariantCultureIgnoreCase));
            scale.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Scale = ((IntegerToken)token).Value.ToInt32();
            };

            var nullToken = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "null", StringComparison.InvariantCultureIgnoreCase));
            nullToken.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsNullable = true;
            };

            var notNullToken = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "not-null", StringComparison.InvariantCultureIgnoreCase));
            notNullToken.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsNullable = false;
            };

            var constraintName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "constraint-name", StringComparison.InvariantCultureIgnoreCase));
            constraintName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.LastConstraintName = ((TextToken)token).Text;
            };

            var pk = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "do-primary-key", StringComparison.InvariantCultureIgnoreCase));
            pk.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.PrimaryKey = new PrimaryKeyInfo
                {
                    Name = tableInfo.LastConstraintName,
                };
            };

            var pkColumnName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "pk-column-name", StringComparison.InvariantCultureIgnoreCase));
            pkColumnName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = new IndexColumnInfo
                {
                    ColumnName = ((TextToken)token).Text,
                };
                primaryKey.Columns.Add(indexColumn);
            };

            var pkColumnAscOrDesc = (ActionNode)allSqlNodes.Single(x =>
                string.Equals(x.Name, "pk-asc-or-desc", StringComparison.InvariantCultureIgnoreCase));
            pkColumnAscOrDesc.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = primaryKey.Columns.Last();

                indexColumn.SortDirection = Enum.Parse<SortDirection>(
                    ((TextToken) token).Text.ToLowerInvariant(), 
                    true);
            };


            //var pkColumnAsc = (ActionNode)allSqlNodes.Single(x =>
            //   string.Equals(x.Name, "asc", StringComparison.InvariantCultureIgnoreCase));
            //pkColumnAsc.Action = (node, token, accumulator) =>
            //{
            //    var tableInfo = accumulator.GetLastResult<TableInfo>();
            //    var primaryKey = tableInfo.PrimaryKey;
            //    var indexColumn = primaryKey.Columns.Last();
            //    indexColumn.SortDirection = SortDirection.Asc;
            //};

            //var pkColumnDesc = (ActionNode)allSqlNodes.Single(x =>
            //   string.Equals(x.Name, "desc", StringComparison.InvariantCultureIgnoreCase));
            //pkColumnDesc.Action = (node, token, accumulator) =>
            //{
            //    var tableInfo = accumulator.GetLastResult<TableInfo>();
            //    var primaryKey = tableInfo.PrimaryKey;
            //    var indexColumn = primaryKey.Columns.Last();
            //    indexColumn.SortDirection = SortDirection.Desc;
            //};

            var fk = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "do-foreign-key", StringComparison.InvariantCultureIgnoreCase));
            fk.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = new ForeignKeyInfo
                {
                    Name = tableInfo.LastConstraintName,
                };
                tableInfo.ForeignKeys.Add(foreignKey);
            };

            var fkTableName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "fk-referenced-table-name", StringComparison.InvariantCultureIgnoreCase));
            fkTableName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyTableName = ((TextToken)token).Text;
                foreignKey.TableName = foreignKeyTableName;
            };

            var fkColumnName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "fk-column-name", StringComparison.InvariantCultureIgnoreCase));
            fkColumnName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyColumnName = ((TextToken)token).Text;
                foreignKey.ColumnNames.Add(foreignKeyColumnName);
            };

            var fkReferencedColumnName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "fk-referenced-column-name", StringComparison.InvariantCultureIgnoreCase));
            fkReferencedColumnName.Action = (node, token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyReferencedColumnName = ((TextToken)token).Text;
                foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);
            };

            // index
            var createUniqueIndex = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "do-create-unique-index", StringComparison.InvariantCultureIgnoreCase));
            createUniqueIndex.Action = (node, token, accumulator) =>
            {
                var index = new IndexInfo
                {
                    IsUnique = true,
                };
                accumulator.AddResult(index);
            };

            var createIndex = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "do-create-index", StringComparison.InvariantCultureIgnoreCase));
            createIndex.Action = (node, token, accumulator) =>
            {
                bool brandNewIndex;

                if (accumulator.Count == 0)
                {
                    brandNewIndex = true;
                }
                else
                {
                    var result = accumulator.Last();
                    if (result is IndexInfo indexInfo)
                    {
                        brandNewIndex = indexInfo.IsCreationFinalized;
                    }
                    else
                    {
                        brandNewIndex = true;
                    }
                }

                if (brandNewIndex)
                {
                    var newIndex = new IndexInfo
                    {
                        IsCreationFinalized = true,
                    };

                    accumulator.AddResult(newIndex);
                }
                else
                {
                    var existingIndexInfo = accumulator.GetLastResult<IndexInfo>();
                    existingIndexInfo.IsCreationFinalized = true;
                }
            };

            var indexName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "index-name", StringComparison.InvariantCultureIgnoreCase));
            indexName.Action = (node, token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.Name = ((TextToken)token).Text;
            };

            var indexTableName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "index-table-name", StringComparison.InvariantCultureIgnoreCase));
            indexTableName.Action = (node, token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.TableName = ((TextToken)token).Text;
            };

            var indexColumnName = (ActionNode)allSqlNodes.Single(x =>
               string.Equals(x.Name, "index-column-name", StringComparison.InvariantCultureIgnoreCase));
            indexColumnName.Action = (node, token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = new IndexColumnInfo
                {
                    ColumnName = ((TextToken)token).Text,
                };
                index.Columns.Add(columnInfo);
            };

            var indexColumnAscOrDesc = (ActionNode)allSqlNodes.Single(x =>
                   string.Equals(x.Name, "index-column-asc-or-desc", StringComparison.InvariantCultureIgnoreCase));
            indexColumnAscOrDesc.Action = (node, token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = index.Columns.Last();
                //columnInfo.SortDirection = SortDirection.Asc;

                columnInfo.SortDirection = Enum.Parse<SortDirection>(
                    ((TextToken)token).Text.ToLowerInvariant(),
                    true);

            };

            //var indexColumnAsc = (ActionNode)allSqlNodes.Single(x =>
            //   string.Equals(x.Name, "index-column-asc", StringComparison.InvariantCultureIgnoreCase));
            //indexColumnAsc.Action = (node, token, accumulator) =>
            //{
            //    var index = accumulator.GetLastResult<IndexInfo>();
            //    var columnInfo = index.Columns.Last();
            //    columnInfo.SortDirection = SortDirection.Asc;
            //};

            //var indexColumnDesc = (ActionNode)allSqlNodes.Single(x =>
            //   string.Equals(x.Name, "index-column-desc", StringComparison.InvariantCultureIgnoreCase));
            //indexColumnDesc.Action = (node, token, accumulator) =>
            //{
            //    var index = accumulator.GetLastResult<IndexInfo>();
            //    var columnInfo = index.Columns.Last();
            //    columnInfo.SortDirection = SortDirection.Desc;
            //};

            #endregion

            var objectNameTokens = allSqlNodes
                .Where(x =>
                    x is TextNode textNode &&
                    x.Name.EndsWith("-name", StringComparison.InvariantCultureIgnoreCase))
                .Cast<TextNode>()
                .ToList();

            // todo: !!! sql lexer will do this job!
            //foreach (var objectNameToken in objectNameTokens)
            //{
            //    objectNameToken.AdditionalChecker = (token, accumulator) =>
            //    {
            //        var textToken = ((TextTokenLab)token).Text.ToUpperInvariant();
            //        return !reservedWords.Contains(textToken);
            //    };
            //}

            var sql =
                @"
CREATE Table my_tab(
    id int NOT NULL,
    name varchar(30) NOT NULL,
    Salary decimal(12, 3) NULL,
    CONSTRAINT [my_tab_pk] PRIMARY KEY(id Desc, [NAME] ASC, salary),
    CONSTRAINT [fk_other] FOREIGN KEY([id]) references other_table(otherId),
    CONSTRAINT fk_cool FOREIGN KEY([id], name) references [other_table](otherId, [birthday])
)

CREATE TABLE [other_table](
    [otherId] nvarchar(10),
    [birthday] [datetime],
    CONSTRAINT pk_otherTable PRIMARY KEY([otherId])
)

CREATE UNIQUE INDEX [UX_name] ON my_tab(id Desc, name, Salary asc)

CREATE INDEX IX_id ON [my_tab](id)

CREATE INDEX [IX_Salary] ON my_tab([salary])

";
            ILexer sqlLexer = new SqlLexer();
            var sqlTokens = sqlLexer.Lexize(sql);

            // Act
            parser.Root = root;
            var sqlResults = parser.Parse(sqlTokens);

            // Assert
            Assert.That(sqlResults, Has.Length.EqualTo(5));

            // create table: my_tab
            var createTableResult = (TableInfo)sqlResults[0];
            Assert.That(createTableResult.Name, Is.EqualTo("my_tab"));
            var tableColumns = createTableResult.Columns;
            Assert.That(tableColumns, Has.Count.EqualTo(3));

            var column = tableColumns[0];
            Assert.That(column.Name, Is.EqualTo("id"));
            Assert.That(column.TypeName, Is.EqualTo("int"));
            Assert.That(column.Precision, Is.Null);
            Assert.That(column.Scale, Is.Null);
            Assert.That(column.IsNullable, Is.False);

            column = tableColumns[1];
            Assert.That(column.Name, Is.EqualTo("name"));
            Assert.That(column.TypeName, Is.EqualTo("varchar"));
            Assert.That(column.Precision, Is.EqualTo(30));
            Assert.That(column.Scale, Is.Null);
            Assert.That(column.IsNullable, Is.False);

            column = tableColumns[2];
            Assert.That(column.Name, Is.EqualTo("Salary"));
            Assert.That(column.TypeName, Is.EqualTo("decimal"));
            Assert.That(column.Precision, Is.EqualTo(12));
            Assert.That(column.Scale, Is.EqualTo(3));
            Assert.That(column.IsNullable, Is.True);

            var tablePrimaryKey = createTableResult.PrimaryKey;
            Assert.That(tablePrimaryKey.Name, Is.EqualTo("my_tab_pk"));
            var pkColumns = tablePrimaryKey.Columns;
            Assert.That(pkColumns, Has.Count.EqualTo(3));

            var pkIndexColumn = pkColumns[0];
            Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("id"));
            Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Desc));

            pkIndexColumn = pkColumns[1];
            Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("NAME"));
            Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Asc));

            pkIndexColumn = pkColumns[2];
            Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("salary"));
            Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Asc));

            var foreignKeys = createTableResult.ForeignKeys;
            Assert.That(foreignKeys, Has.Count.EqualTo(2));

            var tableForeignKey = foreignKeys[0];
            Assert.That(tableForeignKey.Name, Is.EqualTo("fk_other"));
            Assert.That(tableForeignKey.TableName, Is.EqualTo("other_table"));
            CollectionAssert.AreEquivalent(tableForeignKey.ColumnNames, new[] { "id" });
            CollectionAssert.AreEquivalent(tableForeignKey.ReferencedColumnNames, new[] { "otherId" });

            tableForeignKey = foreignKeys[1];
            Assert.That(tableForeignKey.Name, Is.EqualTo("fk_cool"));
            Assert.That(tableForeignKey.TableName, Is.EqualTo("other_table"));
            CollectionAssert.AreEquivalent(tableForeignKey.ColumnNames, new[] { "id", "name" });
            CollectionAssert.AreEquivalent(tableForeignKey.ReferencedColumnNames, new[] { "otherId", "birthday" });

            // create table: other_table
            createTableResult = (TableInfo)sqlResults[1];
            Assert.That(createTableResult.Name, Is.EqualTo("other_table"));
            tableColumns = createTableResult.Columns;
            Assert.That(tableColumns, Has.Count.EqualTo(2));

            column = tableColumns[0];
            Assert.That(column.Name, Is.EqualTo("otherId"));
            Assert.That(column.TypeName, Is.EqualTo("nvarchar"));
            Assert.That(column.Precision, Is.EqualTo(10));
            Assert.That(column.Scale, Is.Null);
            Assert.That(column.IsNullable, Is.True);

            column = tableColumns[1];
            Assert.That(column.Name, Is.EqualTo("birthday"));
            Assert.That(column.TypeName, Is.EqualTo("datetime"));
            Assert.That(column.Precision, Is.Null);
            Assert.That(column.Scale, Is.Null);
            Assert.That(column.IsNullable, Is.True);

            tablePrimaryKey = createTableResult.PrimaryKey;
            Assert.That(tablePrimaryKey.Name, Is.EqualTo("pk_otherTable"));
            pkColumns = tablePrimaryKey.Columns;
            Assert.That(pkColumns, Has.Count.EqualTo(1));

            pkIndexColumn = pkColumns[0];
            Assert.That(pkIndexColumn.ColumnName, Is.EqualTo("otherId"));
            Assert.That(pkIndexColumn.SortDirection, Is.EqualTo(SortDirection.Asc));

            foreignKeys = createTableResult.ForeignKeys;
            Assert.That(foreignKeys, Is.Empty);

            // create index: UX_name
            var createIndexResult = (IndexInfo)sqlResults[2];
            Assert.That(createIndexResult.Name, Is.EqualTo("UX_name"));
            Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
            Assert.That(createIndexResult.IsUnique, Is.True);
            Assert.That(createIndexResult.IsCreationFinalized, Is.True);

            var indexColumns = createIndexResult.Columns;
            Assert.That(indexColumns, Has.Count.EqualTo(3));

            var indexColumnInfo = indexColumns[0];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("id"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Desc));

            indexColumnInfo = indexColumns[1];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("name"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

            indexColumnInfo = indexColumns[2];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("Salary"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

            // create index: IX_id
            createIndexResult = (IndexInfo)sqlResults[3];
            Assert.That(createIndexResult.Name, Is.EqualTo("IX_id"));
            Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
            Assert.That(createIndexResult.IsUnique, Is.False);
            Assert.That(createIndexResult.IsCreationFinalized, Is.True);

            indexColumns = createIndexResult.Columns;
            Assert.That(indexColumns, Has.Count.EqualTo(1));

            indexColumnInfo = indexColumns[0];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("id"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));

            // create index: UX_name
            createIndexResult = (IndexInfo)sqlResults[4];
            Assert.That(createIndexResult.Name, Is.EqualTo("IX_Salary"));
            Assert.That(createIndexResult.TableName, Is.EqualTo("my_tab"));
            Assert.That(createIndexResult.IsUnique, Is.False);
            Assert.That(createIndexResult.IsCreationFinalized, Is.True);

            indexColumns = createIndexResult.Columns;
            Assert.That(indexColumns, Has.Count.EqualTo(1));

            indexColumnInfo = indexColumns[0];
            Assert.That(indexColumnInfo.ColumnName, Is.EqualTo("salary"));
            Assert.That(indexColumnInfo.SortDirection, Is.EqualTo(SortDirection.Asc));
        }
    }
}
