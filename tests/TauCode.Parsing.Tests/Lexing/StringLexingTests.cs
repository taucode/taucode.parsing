using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Lexing
{
    [TestFixture]
    public class StringLexingTests
    {
        [Test]
        public void WatTodo()
        {
            var input = "\"\\n\"";
            ILexer lexer = new MyStringLexer();
            var tokens = lexer.Lexize(input);

            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Text, Is.EqualTo("\n"));
        }
    }
}
