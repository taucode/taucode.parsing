using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    [TextClass("key")]
    public class KeyTextClass : TextClassBase
    {
        public static readonly KeyTextClass Instance = new KeyTextClass();

        private KeyTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}