namespace TauCode.Parsing.Tokens.TextDecorations
{
    public class QuoteTextDecoration : ITextDecoration
    {
        public static readonly QuoteTextDecoration Instance = new QuoteTextDecoration();

        private QuoteTextDecoration()
        {
        }
    }
}
