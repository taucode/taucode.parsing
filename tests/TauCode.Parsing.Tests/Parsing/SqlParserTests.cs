using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Data;
using TauCode.Parsing.Tokens;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Parsing
{
    [TestFixture]
    public class SqlParserTests
    {
        [Test]
        public void SqlParser_ValidInput_Parses()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("SQLiteRealGrammar.txt", true);
            var aideLexer = new AideLexer();
            IParser parser = new Parser();
            var tokens = aideLexer.Lexize(input);
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);

            IBuilder builder = new Builder();

            var sqlRoot = builder.Build("sql tree", results.Cast<BlockDefinitionResult>());
            var allSqlNodes = sqlRoot.FetchTree();

            var exactWordNodes = allSqlNodes
                .Where(x => x is ExactWordNode)
                .Cast<ExactWordNode>()
                .ToList();

            foreach (var exactWordNode in exactWordNodes)
            {
                exactWordNode.IsCaseSensitive = false;
            }

            var reservedWords = exactWordNodes
                .Select(x => x.Word)
                .Distinct()
                .Select(x => x.ToUpperInvariant())
                .ToHashSet();

            var identifiersAsWords = allSqlNodes
                .Where(x => x is WordNode wordNode && x.Name.EndsWith("_name_word"))
                .Cast<WordNode>()
                .ToList();

            #region assign job to nodes

            // table
            var createTable = (ActionNode)allSqlNodes.Single(x => x.Name == "do_create_table");
            createTable.Action = (token, accumulator) =>
            {
                var tableInfo = new TableInfo();
                accumulator.AddResult(tableInfo);
            };

            var tableName = (ActionNode)allSqlNodes.Single(x => x.Name == "table_name");
            tableName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var tableNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "table_name_word");
            tableNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.Name = ((WordToken)token).Word;
            };

            var columnName = (ActionNode)allSqlNodes.Single(x => x.Name == "column_name");
            columnName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var columnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "column_name_word");
            columnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = new ColumnInfo
                {
                    Name = ((WordToken)token).Word,
                };
                tableInfo.Columns.Add(columnInfo);
            };

            var typeName = (ActionNode)allSqlNodes.Single(x => x.Name == "type_name");
            typeName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var columnTypeWord = (ActionNode)allSqlNodes.Single(x => x.Name == "type_name_word");
            columnTypeWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((WordToken)token).Word;
            };

            var scale = (ActionNode)allSqlNodes.Single(x => x.Name == "scale");
            scale.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Scale = ((IntegerToken)token).IntegerValue.ToInt32();
            };

            var precision = (ActionNode)allSqlNodes.Single(x => x.Name == "precision");
            precision.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Precision = ((IntegerToken)token).IntegerValue.ToInt32();
            };

            var nullToken = (ActionNode)allSqlNodes.Single(x => x.Name == "null");
            nullToken.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsNullable = true;
            };

            var notNullToken = (ActionNode)allSqlNodes.Single(x => x.Name == "not_null");
            notNullToken.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsNullable = false;
            };

            var constraintName = (ActionNode)allSqlNodes.Single(x => x.Name == "constraint_name");
            constraintName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.LastConstraintName = ((IdentifierToken)token).Identifier;
            };

            var constraintNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "constraint_name_word");
            constraintNameWord.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var pk = (ActionNode)allSqlNodes.Single(x => x.Name == "do_primary_key");
            pk.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.PrimaryKey = new PrimaryKeyInfo
                {
                    Name = tableInfo.LastConstraintName,
                };
            };

            var pkColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "pk_column_name");
            pkColumnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = new IndexColumnInfo
                {
                    ColumnName = ((IdentifierToken)token).Identifier,
                };
                primaryKey.Columns.Add(indexColumn);
            };

            var pkColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "pk_column_name_word");
            pkColumnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = new IndexColumnInfo
                {
                    ColumnName = ((WordToken)token).Word,
                };
                primaryKey.Columns.Add(indexColumn);
            };

            var pkColumnAsc = (ActionNode)allSqlNodes.Single(x => x.Name == "asc");
            pkColumnAsc.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var pkColumnDesc = (ActionNode)allSqlNodes.Single(x => x.Name == "desc");
            pkColumnDesc.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = primaryKey.Columns.Last();
                indexColumn.SortDirection = SortDirection.Desc;
            };

            var fkColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_column_name");
            fkColumnName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var fkColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_column_name_word");
            fkColumnNameWord.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var fkReferencedColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_referenced_column_name");
            fkReferencedColumnName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var fkReferencedColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_referenced_column_name_word");
            fkReferencedColumnNameWord.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            // index
            var createUniqueIndex = (ActionNode)allSqlNodes.Single(x => x.Name == "do_create_unique_index");
            createUniqueIndex.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var createIndex = (ActionNode)allSqlNodes.Single(x => x.Name == "do_create_index");
            createIndex.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var indexTableName = (ActionNode)allSqlNodes.Single(x => x.Name == "index_table_name");
            indexTableName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var indexTableNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "index_table_name_word");
            indexTableNameWord.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var indexColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_name");
            indexColumnName.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var indexColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_name_word");
            indexColumnNameWord.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var indexColumnAsc = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_asc");
            indexColumnAsc.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };

            var indexColumnDesc = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_desc");
            indexColumnDesc.Action = (token, accumulator) =>
            {
                throw new NotImplementedException();
            };


            #endregion

            foreach (var identifiersAsWord in identifiersAsWords)
            {
                identifiersAsWord.AdditionalChecker = (token, accumulator) =>
                {
                    var wordToken = ((WordToken)token).Word.ToUpperInvariant();
                    return !reservedWords.Contains(wordToken);
                };
            }

            var sql =
@"
CREATE Table my_tab(
    id int NOT NULL,
    name varchar(30) NOT NULL,
    Salary decimal(12, 3) NULL,
    CONSTRAINT [my_tab_pk] PRIMARY KEY(id Desc, [name] ASC, salary),
    CONSTRAINT [fk_other] FOREIGN KEY([id]) references other_table(otherId)
)

CREATE UNIQUE INDEX UX_name ON my_tab(id Desc, name, Salary asc)

CREATE INDEX IX_Salary ON my_tab([salary])

";
            var sqlLexer = new SqlLexer();
            var sqlTokens = sqlLexer.Lexize(sql);

            // Act
            var sqlResults = parser.Parse(sqlRoot, sqlTokens);

            // Assert
            Assert.That(sqlResults, Has.Length.EqualTo(3));

            // create table
            var createTableResult = (TableInfo)sqlResults[0];

            throw new NotImplementedException();
        }
    }
}
