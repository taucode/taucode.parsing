namespace TauCode.Parsing.TextDecorations
{
    public class DoubleQuoteTextDecoration : ITextDecoration
    {
        public static DoubleQuoteTextDecoration Instance { get; } = new DoubleQuoteTextDecoration();
        private DoubleQuoteTextDecoration()
        {
        }
    }
}
