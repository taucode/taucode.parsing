using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class KeyTextClass : ITextClassLab
    {
        public static readonly KeyTextClass Instance = new KeyTextClass();

        private KeyTextClass()
        {
        }

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}