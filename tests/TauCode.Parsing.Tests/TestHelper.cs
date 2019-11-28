using System.IO;
using System.Text;

namespace TauCode.Parsing.Tests
{
    internal static class TestHelper
    {
        internal static void WriteDiff(string actual, string expected, string directory, string fileExtension)
        {
            var actualFileName = $"0-actual.{fileExtension}";
            var expectedFileName = $"1-expected.{fileExtension}";

            var actualFilePath = Path.Combine(directory, actualFileName);
            var expectedFilePath = Path.Combine(directory, expectedFileName);

            File.WriteAllText(actualFilePath, actual, Encoding.UTF8);
            File.WriteAllText(expectedFilePath, expected, Encoding.UTF8);
        }
    }
}
