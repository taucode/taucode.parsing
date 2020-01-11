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

        protected override string TryConvertFromImpl(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
