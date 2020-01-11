namespace TauCode.Parsing.TextDecorations
{
    public class SingleQuoteTextDecoration : ITextDecoration
    {
        public static readonly SingleQuoteTextDecoration Instance = new SingleQuoteTextDecoration();

        private SingleQuoteTextDecoration()
        {
        }
    }
}
