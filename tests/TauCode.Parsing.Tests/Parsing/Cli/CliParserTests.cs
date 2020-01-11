using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lab.Tokens;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Data;
using TauCode.Parsing.Tests.Parsing.Cli.Data.Entries;

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
            _tinyLispLexer = new TinyLispLexerLab();
            _cliLexer = new CliLexer();
        }

        [Test]
        public void CliParser_ValidInput_Parses()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("my-cli");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);
            
            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new TinyLispPseudoReaderLab();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var root = builder.Build(nodeFactory, list);

            IParser parser = new Parser();

            
            var commandText =
                "sd --conn \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var cliTokens = _cliLexer.Lexize(commandText);

            var allNodes = root.FetchTree();

            var mm = (ActionNode) allNodes.Single(x =>
                string.Equals(x.Name, "node-serialize-data", StringComparison.InvariantCultureIgnoreCase));
            mm.Action = (node, token, accumulator) =>
            {
                var command = new CliCommand();
                accumulator.AddResult(command);
            };

            // Act
            var cliResults = parser.Parse(root, cliTokens);

            // Assert
            var cliCommand = (CliCommand) cliResults.Single();

            var commandEntry = (KeyValueCliCommandEntry)cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "connection", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("Server=.;Database=econera.diet.tracking;Trusted_Connection=True;"));

            commandEntry = (KeyValueCliCommandEntry)cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "provider", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("sqlserver"));

            commandEntry = (KeyValueCliCommandEntry)cliCommand.Entries.Single(x =>
                string.Equals(x.Alias, "file", StringComparison.InvariantCultureIgnoreCase));
            Assert.That(
                commandEntry.Value,
                Is.EqualTo("c:/temp/mysqlite.json"));
        }

        [Test]
        public void CliParser_TooManyResults_ThrowsUnexpectedTokenException()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("my-cli");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);
            
            var tokens = _tinyLispLexer.Lexize(input);

            var reader = new TinyLispPseudoReaderLab();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var root = builder.Build(nodeFactory, list);

            IParser parser = new Parser
            {
                WantsOnlyOneResult = true,
            };
            

            var singleCommand = "sd --conn \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var commandText = $"{singleCommand} {singleCommand}";
            var cliTokens = _cliLexer.Lexize(commandText);

            var allNodes = root.FetchTree();

            var mm = (ActionNode)allNodes.Single(x =>
               string.Equals(x.Name, "node-serialize-data", StringComparison.InvariantCultureIgnoreCase));
            mm.Action = (node, token, accumulator) =>
            {
                var command = new CliCommand();
                accumulator.AddResult(command);
            };

            // Act
            var ex = Assert.Throws<UnexpectedTokenException>(() => parser.Parse(root, cliTokens));

            var textToken = (TextTokenLab)ex.Token;
            Assert.That(textToken.Text, Is.EqualTo("sd"));
        }
    }
}
