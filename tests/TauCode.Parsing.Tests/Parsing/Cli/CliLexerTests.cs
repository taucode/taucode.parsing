using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    [TestFixture]
    public class CliLexerTests
    {
        private ILexer _lexer;

        [SetUp]
        public void SetUp()
        {
            _lexer = new CliLexer();
        }

        [Test]
        public void Lexize_ValidInput_ProducesValidTokens()
        {
            // Arrange
            var input = "pub -t one '{\"name\" : \"ak\"}' --repeat 88 -log c:/temp/logs --level 1a-c";

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(10));

            // pub
            var termToken = (TextToken)tokens[0];
            Assert.That(termToken.Class, Is.SameAs(TermTextClass.Instance));
            Assert.That(termToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(termToken.Text, Is.EqualTo("pub"));
            Assert.That(termToken.Position, Is.EqualTo(new Position(0, 0)));
            Assert.That(termToken.ConsumedLength, Is.EqualTo(3));

            // -t
            var keyToken = (TextToken)tokens[1];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            Assert.That(keyToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(keyToken.Text, Is.EqualTo("-t"));
            Assert.That(keyToken.Position, Is.EqualTo(new Position(0, 4)));
            Assert.That(keyToken.ConsumedLength, Is.EqualTo(2));

            // one
            termToken = (TextToken)tokens[2];
            Assert.That(termToken.Class, Is.SameAs(TermTextClass.Instance));
            Assert.That(termToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(termToken.Text, Is.EqualTo("one"));
            Assert.That(termToken.Position, Is.EqualTo(new Position(0, 7)));
            Assert.That(termToken.ConsumedLength, Is.EqualTo(3));

            // '{\"name\" : \"ak\"}'
            var stringToken = (TextToken)tokens[3];
            Assert.That(stringToken.Class, Is.SameAs(StringTextClass.Instance));
            Assert.That(stringToken.Decoration, Is.SameAs(SingleQuoteTextDecoration.Instance));
            Assert.That(stringToken.Text, Is.EqualTo("{\"name\" : \"ak\"}"));
            Assert.That(stringToken.Position, Is.EqualTo(new Position(0, 11)));
            Assert.That(stringToken.ConsumedLength, Is.EqualTo(17));

            // --repeat
            keyToken = (TextToken)tokens[4];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            Assert.That(keyToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(keyToken.Text, Is.EqualTo("--repeat"));
            Assert.That(keyToken.Position, Is.EqualTo(new Position(0, 29)));
            Assert.That(keyToken.ConsumedLength, Is.EqualTo(8));

            // 88
            var intToken = (IntegerToken)tokens[5];
            Assert.That(intToken.Value, Is.EqualTo("88"));
            Assert.That(intToken.Position, Is.EqualTo(new Position(0, 38)));
            Assert.That(intToken.ConsumedLength, Is.EqualTo(2));

            // -log
            keyToken = (TextToken)tokens[6];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            Assert.That(keyToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(keyToken.Text, Is.EqualTo("-log"));
            Assert.That(keyToken.Position, Is.EqualTo(new Position(0, 41)));
            Assert.That(keyToken.ConsumedLength, Is.EqualTo(4));

            // c:/temp/logs
            var pathToken = (TextToken)tokens[7];
            Assert.That(pathToken.Class, Is.SameAs(PathTextClass.Instance));
            Assert.That(pathToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(pathToken.Text, Is.EqualTo("c:/temp/logs"));
            Assert.That(pathToken.Position, Is.EqualTo(new Position(0, 46)));
            Assert.That(pathToken.ConsumedLength, Is.EqualTo(12));

            // --level
            keyToken = (TextToken)tokens[8];
            Assert.That(keyToken.Class, Is.SameAs(KeyTextClass.Instance));
            Assert.That(keyToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(keyToken.Text, Is.EqualTo("--level"));
            Assert.That(keyToken.Position, Is.EqualTo(new Position(0, 59)));
            Assert.That(keyToken.ConsumedLength, Is.EqualTo(7));

            // 1a-c
            termToken = (TextToken)tokens[9];
            Assert.That(termToken.Class, Is.SameAs(TermTextClass.Instance));
            Assert.That(termToken.Decoration, Is.SameAs(NoneTextDecoration.Instance));
            Assert.That(termToken.Text, Is.EqualTo("1a-c"));
            Assert.That(termToken.Position, Is.EqualTo(new Position(0, 67)));
            Assert.That(termToken.ConsumedLength, Is.EqualTo(4));
        }

        [Test]
        [TestCase("-a-b")]
        public void Lexize_KeyWithHyphen_LexizesCorrectly(string input)
        {
            // Arrange

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(1));
            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Class, Is.SameAs(KeyTextClass.Instance));
            Assert.That(textToken.Decoration, Is.EqualTo(NoneTextDecoration.Instance));
            Assert.That(textToken.Text, Is.EqualTo("-a-b"));
        }

        [Test]
        [TestCase(".")]
        [TestCase("..")]
        [TestCase("-")]
        [TestCase("--")]
        [TestCase("---")]
        [TestCase("--fo-")]
        [TestCase("-fo-")]
        [TestCase("---foo")]
        public void Lexize_StrangePath_LexizesAsPath(string input)
        {
            // Arrange

            // Act
            var tokens = _lexer.Lexize(input);

            // Assert
            Assert.That(tokens, Has.Count.EqualTo(1));
            var textToken = (TextToken)tokens.Single();
            Assert.That(textToken.Class, Is.SameAs(PathTextClass.Instance));
            Assert.That(textToken.Decoration, Is.EqualTo(NoneTextDecoration.Instance));
            Assert.That(textToken.Text, Is.EqualTo(input));
        }

        [Test]
        [TestCase("я")]
        public void Lexize_UnexpectedSymbol_ThrowsLexingException(string input)
        {
            // Arrange

            // Act
            var ex = Assert.Throws<LexingException>(() => _lexer.Lexize(input));
            

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"Unexpected char: '{input[0]}'."));
            Assert.That(ex.Position, Is.EqualTo(Position.Zero));
        }
    }
}
