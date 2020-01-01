using NUnit.Framework;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.Tests.Parsing.Cli.TextDecorations;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

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
            var termToken = (TextToken)tokens[0];
            Assert.That(termToken.Class, Is.SameAs(TermTextClass.Instance));
            Assert.That(termToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(termToken.Text, Is.EqualTo("pub"));

            // -t
            var keyToken = (TextToken)tokens[1];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            var hyphenTextDecoration = (HyphenTextDecoration)keyToken.Decoration;
            Assert.That(hyphenTextDecoration.HyphenCount, Is.EqualTo(1));
            Assert.That(keyToken.Text, Is.EqualTo("t"));

            // one
            termToken = (TextToken)tokens[2];
            Assert.That(termToken.Class, Is.SameAs(TermTextClass.Instance));
            Assert.That(termToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(termToken.Text, Is.EqualTo("one"));

            // '{\"name\" : \"ak\"}'
            var stringToken = (TextToken)tokens[3];
            Assert.That(stringToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(stringToken.Decoration, Is.SameAs(SingleQuoteTextDecoration.Instance));
            Assert.That(stringToken.Text, Is.EqualTo("{\"name\" : \"ak\"}"));

            // --repeat
            keyToken = (TextToken)tokens[4];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            hyphenTextDecoration = (HyphenTextDecoration)keyToken.Decoration;
            Assert.That(hyphenTextDecoration.HyphenCount, Is.EqualTo(2));
            Assert.That(keyToken.Text, Is.EqualTo("repeat"));

            // 88
            var intToken = (IntegerToken)tokens[5];
            Assert.That(intToken.Value, Is.EqualTo("88"));

            // -log
            keyToken = (TextToken)tokens[6];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            hyphenTextDecoration = (HyphenTextDecoration)keyToken.Decoration;
            Assert.That(hyphenTextDecoration.HyphenCount, Is.EqualTo(1));
            Assert.That(keyToken.Text, Is.EqualTo("log"));

            // c:/temp/logs
            var pathToken = (TextToken)tokens[7];
            Assert.That(pathToken.Class, Is.SameAs(PathTextClass.Instance));
            Assert.That(pathToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(pathToken.Text, Is.EqualTo("c:/temp/logs"));

            // --level
            keyToken = (TextToken)tokens[8];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            hyphenTextDecoration = (HyphenTextDecoration)keyToken.Decoration;
            Assert.That(hyphenTextDecoration.HyphenCount, Is.EqualTo(2));
            Assert.That(keyToken.Text, Is.EqualTo("level"));

            // 1a-c
            termToken = (TextToken)tokens[9];
            Assert.That(termToken.Class, Is.SameAs(TermTextClass.Instance));
            Assert.That(termToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(termToken.Text, Is.EqualTo("1a-c"));
        }
    }
}
