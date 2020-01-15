namespace TauCode.Parsing.Lexing
{
    public static class LexingContextExtensions
    {
        public static void Advance(this LexingContext context, int indexShift, int lineShift, int column)
        {
            context.Index += indexShift;
            context.Line += lineShift;
            context.Column = column;

            context.IncreaseVersion();
        }

        public static void AdvanceByChar(this LexingContext context)
        {
            context.Index++;
            context.Column++;

            context.IncreaseVersion();
        }
    }
}
