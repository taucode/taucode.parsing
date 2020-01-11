using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    [TextClass("path")]
    public class PathTextClass : TextClassBaseLab
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }

        public override string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
