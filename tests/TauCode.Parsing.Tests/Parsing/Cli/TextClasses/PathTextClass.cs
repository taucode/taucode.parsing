using TauCode.Parsing.Old;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class PathTextClass : IOldTextClass
    {
        public static readonly PathTextClass Instance = new PathTextClass();

        private PathTextClass()
        {
        }
    }
}
