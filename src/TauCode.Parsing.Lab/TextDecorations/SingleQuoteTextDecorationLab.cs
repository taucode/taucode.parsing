namespace TauCode.Parsing.Lab.TextDecorations
{
    public class SingleQuoteTextDecorationLab : ITextDecorationLab
    {
        public static SingleQuoteTextDecorationLab Instance { get; } = new SingleQuoteTextDecorationLab();
        private SingleQuoteTextDecorationLab()
        {
        }
    }
}
