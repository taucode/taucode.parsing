using NUnit.Framework;
using System;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestWat()
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
                new SymbolToken(')'),
            };

            IParser parser = new Parser();

            // Act
            var result = parser.Parse(tokens);

            // Assert
            throw new NotImplementedException();
        }
    }
}
