using TauCode.Parsing.Old;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class TermTextClass : IOldTextClass
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }
    }
}
