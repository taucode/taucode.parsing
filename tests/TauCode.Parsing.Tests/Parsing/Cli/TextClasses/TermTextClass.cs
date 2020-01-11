using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    [TextClass("term")]
    public class TermTextClass : TextClassBaseLab
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }

        protected override string TryConvertFromImpl(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}
