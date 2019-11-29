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
    public class SQLiteParser
    {
        public static SQLiteParser Instance { get; } = new SQLiteParser();

        private readonly INode _root;
        private readonly IParser _parser;

        private SQLiteParser()
        {
            _root = this.BuildRoot();
            _parser = new Parser();
        }

        private INode BuildRoot()
        {
            var input = this.GetType().Assembly.GetResourceText("SQLiteRealGrammar.txt", true);
            var aideLexer = new AideLexer();
            IParser parser = new Parser();
            var tokens = aideLexer.Lexize(input);
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);

            IBuilder builder = new Builder();

            var sqlRoot = builder.Build("SQLite", results.Cast<BlockDefinitionResult>());
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
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.Name = ((IdentifierToken)token).Identifier;
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
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = new ColumnInfo
                {
                    Name = ((IdentifierToken)token).Identifier,
                };
                tableInfo.Columns.Add(columnInfo);
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
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((IdentifierToken)token).Identifier;
            };

            var columnTypeWord = (ActionNode)allSqlNodes.Single(x => x.Name == "type_name_word");
            columnTypeWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.TypeName = ((WordToken)token).Word;
            };

            var precision = (ActionNode)allSqlNodes.Single(x => x.Name == "precision");
            precision.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Precision = ((IntegerToken)token).IntegerValue.ToInt32();
            };

            var scale = (ActionNode)allSqlNodes.Single(x => x.Name == "scale");
            scale.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Scale = ((IntegerToken)token).IntegerValue.ToInt32();
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

            var inlinePrimaryKey = (ActionNode)allSqlNodes.Single(x => x.Name == "inline_primary_key");
            inlinePrimaryKey.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.IsPrimaryKey = true;
            };

            var defaultNull = (ActionNode)allSqlNodes.Single(x => x.Name == "default_null");
            defaultNull.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Default = "NULL";
            };

            var defaultInteger = (ActionNode)allSqlNodes.Single(x => x.Name == "default_integer");
            defaultInteger.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Default = ((IntegerToken)token).IntegerValue;
            };

            var defaultString = (ActionNode)allSqlNodes.Single(x => x.Name == "default_string");
            defaultString.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var columnInfo = tableInfo.Columns.Last();
                columnInfo.Default = $"'{((StringToken)token).Value}'";
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
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                tableInfo.LastConstraintName = ((WordToken)token).Word;
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
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = primaryKey.Columns.Last();
                indexColumn.SortDirection = SortDirection.Asc;
            };

            var pkColumnDesc = (ActionNode)allSqlNodes.Single(x => x.Name == "desc");
            pkColumnDesc.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var primaryKey = tableInfo.PrimaryKey;
                var indexColumn = primaryKey.Columns.Last();
                indexColumn.SortDirection = SortDirection.Desc;
            };

            var fk = (ActionNode)allSqlNodes.Single(x => x.Name == "do_foreign_key");
            fk.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = new ForeignKeyInfo
                {
                    Name = tableInfo.LastConstraintName,
                };
                tableInfo.ForeignKeys.Add(foreignKey);
            };

            var fkTableName = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_referenced_table_name");
            fkTableName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyTableName = ((IdentifierToken)token).Identifier;
                foreignKey.TableName = foreignKeyTableName;
            };

            var fkTableNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_referenced_table_name_word");
            fkTableNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyTableName = ((WordToken)token).Word;
                foreignKey.TableName = foreignKeyTableName;
            };

            var fkColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_column_name");
            fkColumnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyColumnName = ((IdentifierToken)token).Identifier;
                foreignKey.ColumnNames.Add(foreignKeyColumnName);
            };

            var fkColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_column_name_word");
            fkColumnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyColumnName = ((WordToken)token).Word;
                foreignKey.ColumnNames.Add(foreignKeyColumnName);
            };

            var fkReferencedColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_referenced_column_name");
            fkReferencedColumnName.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyReferencedColumnName = ((IdentifierToken)token).Identifier;
                foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);
            };

            var fkReferencedColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "fk_referenced_column_name_word");
            fkReferencedColumnNameWord.Action = (token, accumulator) =>
            {
                var tableInfo = accumulator.GetLastResult<TableInfo>();
                var foreignKey = tableInfo.ForeignKeys.Last();
                var foreignKeyReferencedColumnName = ((WordToken)token).Word;
                foreignKey.ReferencedColumnNames.Add(foreignKeyReferencedColumnName);
            };

            // index
            var createUniqueIndex = (ActionNode)allSqlNodes.Single(x => x.Name == "do_create_unique_index");
            createUniqueIndex.Action = (token, accumulator) =>
            {
                var index = new IndexInfo
                {
                    IsUnique = true,
                };
                accumulator.AddResult(index);
            };

            var createIndex = (ActionNode)allSqlNodes.Single(x => x.Name == "do_create_index");
            createIndex.Action = (token, accumulator) =>
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

            var indexName = (ActionNode)allSqlNodes.Single(x => x.Name == "index_name");
            indexName.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.Name = ((IdentifierToken)token).Identifier;
            };

            var indexNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "index_name_word");
            indexNameWord.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.Name = ((WordToken)token).Word;
            };

            var indexTableName = (ActionNode)allSqlNodes.Single(x => x.Name == "index_table_name");
            indexTableName.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.TableName = ((IdentifierToken)token).Identifier;
            };

            var indexTableNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "index_table_name_word");
            indexTableNameWord.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                index.TableName = ((WordToken)token).Word;
            };

            var indexColumnName = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_name");
            indexColumnName.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = new IndexColumnInfo
                {
                    ColumnName = ((IdentifierToken)token).Identifier,
                };
                index.Columns.Add(columnInfo);
            };

            var indexColumnNameWord = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_name_word");
            indexColumnNameWord.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = new IndexColumnInfo
                {
                    ColumnName = ((WordToken)token).Word,
                };
                index.Columns.Add(columnInfo);
            };

            var indexColumnAsc = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_asc");
            indexColumnAsc.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = index.Columns.Last();
                columnInfo.SortDirection = SortDirection.Asc;
            };

            var indexColumnDesc = (ActionNode)allSqlNodes.Single(x => x.Name == "index_column_desc");
            indexColumnDesc.Action = (token, accumulator) =>
            {
                var index = accumulator.GetLastResult<IndexInfo>();
                var columnInfo = index.Columns.Last();
                columnInfo.SortDirection = SortDirection.Desc;
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

            return sqlRoot;
        }

        public object[] Parse(string sql)
        {
            ILexer lexer = new SqlLexer();
            var tokens = lexer.Lexize(sql);

            var results = _parser.Parse(_root, tokens);
            return results;
        }
    }
}
