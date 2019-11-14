using NUnit.Framework;
using System;
using TauCode.Parsing.Aide;
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

            throw new NotImplementedException();
            //// Act
            //IParser parser = new AideParser();
            //var context = parser.Parse(tokens);

            //// Assert
            //var results = context
            //    .ToArray()
            //    .Cast<IAideResult>()
            //    .ToList();

            //Assert.That(results, Has.Count.EqualTo(8));


            //for (var i = 0; i < results.Count; i++)
            //{
            //    var result = results[i];
            //    var resultString = result.ToAideResultFormat();
            //    var expectedResultString = this.GetType().Assembly.GetResourceText($"Result{i}.txt", true);

            //    Assert.That(resultString, Is.EqualTo(expectedResultString));
            //}
        }
    }
}
