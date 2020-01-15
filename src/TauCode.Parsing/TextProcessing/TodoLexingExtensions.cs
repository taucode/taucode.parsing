namespace TauCode.Parsing.TextProcessing
{
    public static class TodoLexingExtensions
    {
        public static int GetIndex(this LexingContext context) => context.Index; // todo temp
        public static char GetCurrentChar(this LexingContext context) => context.Text[context.Index]; // todo temp

        public static void Advance(this LexingContext context, int indexShift, int lineShift, int column)
        {
            context.Index += indexShift;
            context.Line += lineShift;
            context.Column = column;

            context.IncreaseVersion();
        }
            

        public static Position GetCurrentPosition(this LexingContext context) => new Position(context.Line, context.Column);

        //public static bool IsEnd(this LexingContext context) => context.Index == context.Length;

        public static void AdvanceByChar(this LexingContext context)
        {
            context.Index++;
            context.Column++;

            context.IncreaseVersion();
        }
    }
}
