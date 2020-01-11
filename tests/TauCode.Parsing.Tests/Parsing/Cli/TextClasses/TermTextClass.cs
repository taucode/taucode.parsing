using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    public class TermTextClass : ITextClassLab
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }

        public string TryConvertFrom(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
