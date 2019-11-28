using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Results;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Cli
{
    [TestFixture]
    public class CliTests
    {
        [Test]
        public void TodoWat()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("CliSerializeMetadataGrammar.txt", true);

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);

            // Act
            IParser parser = new Parser();
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);
            
            IBuilder builder = new Builder();
            var cliRoot = builder.Build("CLI", results.Cast<BlockDefinitionResult>());

            var commandText = this.GetType().Assembly.GetResourceText("CliCommand.txt", true);
            ILexer commandLexer = new CliLexer();
            var commandTokens = commandLexer.Lexize(commandText);

            var commandResults = parser.Parse(cliRoot, commandTokens);

            // Assert
            throw new NotImplementedException("good, go on!");
        }
    }
}
