using TauCode.Parsing.Old;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class KeyTextClass : IOldTextClass
    {
        public static readonly KeyTextClass Instance = new KeyTextClass();

        private KeyTextClass()
        {
        }
    }
}