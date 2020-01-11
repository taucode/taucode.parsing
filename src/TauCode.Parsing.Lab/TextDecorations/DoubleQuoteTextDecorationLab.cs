namespace TauCode.Parsing.Lab.TextDecorations
{
    public class DoubleQuoteTextDecorationLab : ITextDecorationLab
    {
        public static DoubleQuoteTextDecorationLab Instance { get; } = new DoubleQuoteTextDecorationLab();
        private DoubleQuoteTextDecorationLab()
        {
        }
    }
}
