using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Results;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideParserTests
    {
        [Test]
        public void Parse_SqlGrammar_Parses()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("AllBlocks.txt", true);

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);

            // Act
            IParser parser = new Parser();
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);

            // Assert
            var aideResults = results
                .Cast<IAideResult>()
                .ToList();

            Assert.That(aideResults, Has.Count.EqualTo(8));

            for (var i = 0; i < aideResults.Count; i++)
            {
                var aideResult = aideResults[i];
                var resultString = aideResult.ToAideResultFormat();
                var expectedResultString = this.GetType().Assembly.GetResourceText($"Result{i}.txt", true);

                Assert.That(resultString, Is.EqualTo(expectedResultString));
            }
        }

        [Test]
        public void Parse_CliGrammar_Parses()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("CliGrammar.txt", true);

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);

            // Act
            IParser parser = new Parser();
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);

            // Assert
            var aideResults = results
                .Cast<IAideResult>()
                .ToList();

            Assert.That(aideResults, Has.Count.EqualTo(1));
            var aideResult = aideResults.Single();

            var resultString = aideResult.ToAideResultFormat();
            var expectedResultString = this.GetType().Assembly.GetResourceText($"CliGrammarExpected.txt", true);

            TestHelper.WriteDiff(resultString, expectedResultString, "c:/temp/70-a-wtf", "txt");

            Assert.That(resultString, Is.EqualTo(expectedResultString));
        }
    }
}
