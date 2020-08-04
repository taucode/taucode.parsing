using TauCode.Extensions;
using TauCode.Parsing.TextClasses;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    [TextClass("path")]
    public class PathTextClass : TextClassBase
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            if (anotherClass.IsIn(
                TermTextClass.Instance,
                KeyTextClass.Instance,
                StringTextClass.Instance
            ))
            {
                return text;
            }

            return null;
        }
    }
}
