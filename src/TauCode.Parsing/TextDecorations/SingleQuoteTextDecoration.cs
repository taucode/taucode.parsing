namespace TauCode.Parsing.TextDecorations
{
    public class SingleQuoteTextDecoration : ITextDecoration
    {
        public static SingleQuoteTextDecoration Instance { get; } = new SingleQuoteTextDecoration();
        private SingleQuoteTextDecoration()
        {
        }
    }
}
