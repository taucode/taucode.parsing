using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class TermTextClass : ITextClass
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }
    }
}
