namespace TauCode.Parsing.Tokens.TextDecorations
{
    public class BackQuoteTextDecoration : ITextDecoration
    {
        public static readonly BackQuoteTextDecoration Instance = new BackQuoteTextDecoration();

        private BackQuoteTextDecoration()
        {   
        }
    }
}
