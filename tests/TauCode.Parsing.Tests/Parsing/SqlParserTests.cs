using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
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
    CONSTRAINT [my_tab_pk] PRIMARY KEY(id Desc, [name] ASC, salary)
)";
            var sqlLexer = new SqlLexer();
            var sqlTokens = sqlLexer.Lexize(sql);

            // Act
            var sqlResults = parser.Parse(sqlRoot, sqlTokens);

            // Assert
            throw new NotImplementedException();
        }
    }
}
