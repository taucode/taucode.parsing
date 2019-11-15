using NUnit.Framework;
using System.IO;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Results2;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Aide
{
    [TestFixture]
    public class AideParserTests
    {
        [Test]
        public void Parse_ValidInput_Parses()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("AllBlocks.txt", true);

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);

            // Act
            IParser2 parser = new Parser2();
            var aideRoot = AideHelper.BuildParserRoot();
            var results = parser.Parse(aideRoot, tokens);

            // Assert
            var aideResults = results
                .Cast<IAideResult2>()
                .ToList();

            Assert.That(aideResults, Has.Count.EqualTo(8));

            for (var i = 0; i < aideResults.Count; i++)
            {
                var aideResult = aideResults[i];
                var resultString = aideResult.ToAideResultFormat();
                var expectedResultString = this.GetType().Assembly.GetResourceText($"Result{i}.txt", true);

                if (resultString != expectedResultString)
                {
                    File.WriteAllText("c:/temp/aza0.txt", resultString);
                    File.WriteAllText("c:/temp/aza1.txt", expectedResultString);
                }

                Assert.That(resultString, Is.EqualTo(expectedResultString));
            }
        }
    }
}
