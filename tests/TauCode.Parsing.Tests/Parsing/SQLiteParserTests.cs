using NUnit.Framework;
using System.Text;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Parsing
{
    [TestFixture]
    public class SQLiteParserTests
    {
        [Test]
        public void Parse_RealSQLite_ReturnsValidResult()
        {
            // Arrange
            var sql = this.GetType().Assembly.GetResourceText("sqlite-real.sql", true);

            // Act
            var sqliteResults = SQLiteParser.Instance.Parse(sql);

            // Assert
            Assert.That(sqliteResults, Has.Length.EqualTo(5));

            var sb = new StringBuilder();

            foreach (var result in sqliteResults)
            {
                sb.Append(result);
                sb.AppendLine();
                sb.AppendLine();
            }

            var actualText = sb.ToString();
            var expectedText = this.GetType().Assembly.GetResourceText("sqlite-expected.sql", true);

            Assert.That(actualText, Is.EqualTo(expectedText));
        }
    }
}
