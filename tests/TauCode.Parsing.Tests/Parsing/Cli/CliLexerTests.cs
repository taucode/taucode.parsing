using NUnit.Framework;
using TauCode.Parsing.Tests.Parsing.Cli.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    [TestFixture]
    public class CliLexerTests
    {
        // todo: more tests. e.g. 
        // "-" - path
        // "." - path
        // "--" - path
        // etc.
        [Test]
        public void Lexize_ValidInput_ProducesValidTokens()
        {
            // Arrange
            var input = "pub -t one '{\"name\" : \"ak\"}' --repeat 88 -log c:/temp/logs --level 1a-c";
            var lexer = new CliLexer();

            // Act
            var tokens = lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(10));

            // pub
            var termToken = (TermToken)tokens[0];
            Assert.That(termToken.Value, Is.EqualTo("pub"));

            // -t
            var keyToken = (KeyToken)tokens[1];
            Assert.That(keyToken.KeyName, Is.EqualTo("t"));
            Assert.That(keyToken.Prefix, Is.EqualTo("-"));

            // one
            termToken = (TermToken)tokens[2];
            Assert.That(termToken.Value, Is.EqualTo("one"));

            // '{\"name\" : \"ak\"}'
            var stringToken = (TextToken)tokens[3];
            Assert.That(stringToken.Text, Is.EqualTo("{\"name\" : \"ak\"}"));

            // --repeat
            keyToken = (KeyToken)tokens[4];
            Assert.That(keyToken.KeyName, Is.EqualTo("repeat"));
            Assert.That(keyToken.Prefix, Is.EqualTo("--"));

            // 88
            var intToken = (IntegerToken)tokens[5];
            Assert.That(intToken.Value, Is.EqualTo("88"));

            // -log
            keyToken = (KeyToken)tokens[6];
            Assert.That(keyToken.KeyName, Is.EqualTo("log"));
            Assert.That(keyToken.Prefix, Is.EqualTo("-"));

            // c:/temp/logs
            var pathToken = (PathToken)tokens[7];
            Assert.That(pathToken.Value, Is.EqualTo("c:/temp/logs"));

            // --level
            keyToken = (KeyToken)tokens[8];
            Assert.That(keyToken.KeyName, Is.EqualTo("level"));
            Assert.That(keyToken.Prefix, Is.EqualTo("--"));

            // 1a
            termToken = (TermToken)tokens[9];
            Assert.That(termToken.Value, Is.EqualTo("1a-c"));
        }
    }
}
