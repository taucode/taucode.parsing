using NUnit.Framework;
using System.Linq;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Old.Tests.Lexing
{
    [TestFixture]
    public class LexingHelperTests
    {
        [Test]
        public void IsStandardPunctuationChar_Punctuation_ReturnsTrue()
        {
            // Arrange
            var punctuations = new char[]
            {
                '!',
                '"',
                '#',
                '%',
                '&',
                '\'',
                '(',
                ')',
                '*',
                ',',
                '-',
                '.',
                '/',
                ':',
                ';',
                '?',
                '@',
                '[',
                '\\',
                ']',
                '{',
                '}',
                '|',
                '`',
            };

            // Act
            var correct = punctuations.All(LexingHelper.IsStandardPunctuationChar);

            // Assert
            Assert.That(correct, Is.True);
        }
    }
}
