using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Results;
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
            var sql =
@"
CREATE TABLE my_tab(
    id int NOT NULL,
    name varchar(30) NOT NULL,
    Salary decimal(12, 3) NULL,
    CONSTRAINT [my_tab_pk] PRIMARY KEY(id DESC, [name] ASC, salary)
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
