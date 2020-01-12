namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    [TextClass("term")]
    public class TermTextClass : TextClassBase
    {
        public static readonly TermTextClass Instance = new TermTextClass();

        private TermTextClass()
        {   
        }

        protected override string TryConvertFromImpl(string text, ITextClass anotherClass) => null;
    }
}
