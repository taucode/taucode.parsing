using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Building;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tests.Parsing.Cli.Data;
using TauCode.Parsing.Tests.Parsing.Cli.Data.Entries;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    // todo clean up
    [TestFixture]
    public class CliParserTests
    {
        [Test]
        public void CliParser_ValidInput_Parses()
        {
            // Arrange
            var nodeFactory = new CliNodeFactory("my-cli");
            var input = this.GetType().Assembly.GetResourceText("cli-grammar.lisp", true);
            ILexer lexer = new TinyLispLexer();
            var tokens = lexer.Lexize(input);

            var reader = new TinyLispPseudoReader();
            var list = reader.Read(tokens);
            IBuilder builder = new Builder();
            var root = builder.Build(nodeFactory, list);

            IParser parser = new Parser();

            ILexer cliLexer = new CliLexer();
            //var commandText = this.GetType().Assembly.GetResourceText("CliCommand.txt", true);
            var commandText =
                "sd --conn \"Server=.;Database=econera.diet.tracking;Trusted_Connection=True;\" --provider sqlserver -f c:/temp/mysqlite.json";
            var cliTokens = cliLexer.Lexize(commandText);

            var allNodes = root.FetchTree();

            var mm = (ActionNode) allNodes.Single(x =>
                string.Equals(x.Name, "node-serialize-data", StringComparison.InvariantCultureIgnoreCase));
            mm.Action = (node, token, accumulator) =>
            {
                var command = new CliCommand();
                accumulator.AddResult(command);
            };

            //void AddKey(ActionNode actionNode, IToken token, IResultAccumulator accumulator)
            //{
            //    throw new NotImplementedException();
            //    //var command = accumulator.GetLastResult<MmCommand>();
            //    //command.Entries.Add(new MmCommandEntry());

            //    //var entry = command.Entries.Last();
            //    //entry.Key = ((TextToken)token).Text;
            //}

            //void AddValue(ActionNode actionNode, IToken token, IResultAccumulator accumulator)
            //{
            //    throw new NotImplementedException();
            //    //var command = accumulator.GetLastResult<MmCommand>();
            //    //var entry = command.Entries.Last();
            //    //entry.Value = ((TextToken)token).Text;
            //}

            //var keyNodes = allNodes
            //    .Where(x => x.Name?.EndsWith("-key", StringComparison.CurrentCultureIgnoreCase) ?? false)
            //    .Cast<ActionNode>()
            //    .ToList();

            //keyNodes.ForEach(x => x.Action = AddKey);

            //var valueNodes = allNodes
            //    .Where(x => x.Name?.EndsWith("-value", StringComparison.CurrentCultureIgnoreCase) ?? false)
            //    .Cast<ActionNode>()
            //    .ToList();

            //valueNodes.ForEach(x => x.Action = AddValue);

            // Act
            var cliResults = parser.Parse(root, cliTokens);

            // Assert
            var cliCommand = (CliCommand) cliResults.Single();

            //var mmResult = (MmCommand)cliResults.Single();

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

            //commandEntry = mmResult.Entries.Single(x => x.Key == "provider");
            //Assert.That(commandEntry.Value, Is.EqualTo("sqlserver"));

            //commandEntry = mmResult.Entries.Single(x => x.Key == "to");
            //Assert.That(commandEntry.Value, Is.EqualTo("sqlite"));

            //commandEntry = mmResult.Entries.Single(x => x.Key == "target");
            //Assert.That(commandEntry.Value, Is.EqualTo("c:/temp/mysqlite.json"));
        }
    }
}
