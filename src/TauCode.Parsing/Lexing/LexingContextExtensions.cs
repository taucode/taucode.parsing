using System;

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

        public static bool StartsWith(this LexingContext context, string start)
        {
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            var remaining = context.GetRemainingCharCount();
            if (remaining < start.Length)
            {
                return false;
            }

            var text = context.Text;
            var initialIndex = context.Index;
            for (var i = 0; i < start.Length; i++)
            {
                var c1 = text[initialIndex + i];
                var c2 = start[i];
                if (c1 != c2)
                {
                    return false;
                }
            }

            return true;
        }

        public static int GetRemainingCharCount(this LexingContext context)
        {
            return context.Length - context.Index;
        }

    }
}
