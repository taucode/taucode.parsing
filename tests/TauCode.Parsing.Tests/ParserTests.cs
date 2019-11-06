using NUnit.Framework;
using System.Collections.Generic;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestWatTodo()
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

            IParser parser = new Parser();

            // Act
            var result = parser.Parse(tokens);

            // Assert
            var table = result.Get("table");

            var name = (string)table.Name;
            var columns = (List<dynamic>)table.Columns;

            Assert.That(name, Is.EqualTo("my_tab"));

            Assert.That(columns, Has.Count.EqualTo(2));

            var column = columns[0];
            Assert.That(column.Name, Is.EqualTo("id"));
            Assert.That(column.Type, Is.EqualTo("integer"));

            column = columns[1];
            Assert.That(column.Name, Is.EqualTo("name"));
            Assert.That(column.Type, Is.EqualTo("text"));
        }
    }
}
