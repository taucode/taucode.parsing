using NUnit.Framework;
using System;
using System.Linq;
using TauCode.Parsing.Aide;
using TauCode.Parsing.Aide.Building;
using TauCode.Parsing.Aide.Results;
using TauCode.Utils.Extensions;

namespace TauCode.Parsing.Tests.Building
{
    [TestFixture]
    public class BuilderTests
    {
        [Test]
        public void ToDo()
        {
            // Arrange
            var input = this.GetType().Assembly.GetResourceText("AllBlocks.txt", true);

            var lexer = new AideLexer();
            var tokens = lexer.Lexize(input);
            IParser parser = new AideParser();
            var context = parser.Parse(tokens);

            var builder = new Builder();
            var buildEnvironment = new BuildEnvironment();
            
            // Act
            builder.BuildMainBlock(
                context
                    .ToArray()
                    .Cast<IAideResult>()
                    .ToArray(),
                buildEnvironment);

            // Assert
            throw new NotImplementedException();
        }
    }
}
