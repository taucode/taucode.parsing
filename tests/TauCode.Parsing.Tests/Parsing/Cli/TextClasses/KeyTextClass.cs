using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class KeyTextClass : ITextClass
    {
        public static readonly KeyTextClass Instance = new KeyTextClass();

        private KeyTextClass()
        {
        }
    }
}