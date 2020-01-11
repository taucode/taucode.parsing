using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class PathTextClass : ITextClassLab
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
