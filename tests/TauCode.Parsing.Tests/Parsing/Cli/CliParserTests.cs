using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.Data;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    [TestFixture]
    public class CliParserTests
    {
        private ILexer _tinyLispLexer;
        private ILexer _cliLexer;

        [SetUp]
        public void SetUp()
        {
            _tinyLispLexer = new TinyLispLexer();
            _cliLexer = new CliLexer();
        }

        [Test]
        public void CliParser_ValidInput_Parses()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("CLI node family");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);

            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            var root = builder.Build(nodeFactory, list);

            IParser parser = new Parser
            {
                Root = root,
            };

            var commandText =
                "sd --connection \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var cliTokens = _cliLexer.Lexize(commandText);

            // Act
            parser.Root = root;
            var cliResults = parser.Parse(cliTokens);

            // Assert
            var cliCommand = (CliCommand)cliResults.Single();

            var commandEntry = cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "connection", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("Server=.;Database=econera.diet.tracking;Trusted_Connection=True;"));

            commandEntry = cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "provider", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("sqlserver"));

            commandEntry = cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "file", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("c:/temp/mysqlite.json"));
        }

        [Test]
        public void CliParser_TooManyResults_ThrowsUnexpectedTokenException()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("CLI node family");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);

            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            var root = builder.Build(nodeFactory, list);

            IParser parser = new Parser
            {
                WantsOnlyOneResult = true,
                Root = root,
            };


            var singleCommand = "sd --connection \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var commandText = $"{singleCommand} {singleCommand}";
            var cliTokens = _cliLexer.Lexize(commandText);

            // Act
            parser.Root = root;
            var ex = Assert.Throws<UnexpectedTokenException>(() => parser.Parse(cliTokens));

            var textToken = (TextToken)ex.Token;
            Assert.That(textToken.Text, Is.EqualTo("sd"));
        }

        [Test]
        public void CliParser_BadKey_ThrowsFallbackNodeAcceptedTokenException()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("CLI node family");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);

            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);
            ITreeBuilder builder = new TreeBuilder();
            var root = builder.Build(nodeFactory, list);

            IParser parser = new Parser
            {
                WantsOnlyOneResult = true,
                Root = root,
            };


            var singleCommand = "sd -bad-key \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var commandText = $"{singleCommand} {singleCommand}";
            var cliTokens = _cliLexer.Lexize(commandText);

            // Act
            parser.Root = root;
            var ex = Assert.Throws<FallbackNodeAcceptedTokenException>(() => parser.Parse(cliTokens));

            var textToken = (TextToken)ex.Token;
            Assert.That(textToken.Text, Is.EqualTo("-bad-key"));
        }
    }
}
