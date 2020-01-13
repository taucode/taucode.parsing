using System;

namespace TauCode.Parsing.TextProcessing
{
    public static class TextProcessingContextExtensions
    {
        public static void ReleaseGenerationAndGetMetrics(
            this ITextProcessingContext context,
            out int indexShift,
            out int lineShift,
            out int currentColumn)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var newIndex = context.GetIndex();
            var newLine = context.Line;
            currentColumn = context.Column;

            context.ReleaseGeneration();

            var oldIndex = context.GetIndex();
            var oldLine = context.Line;

            indexShift = newIndex - oldIndex;
            lineShift = newLine - oldLine;
        }

        public static int GetIndex(this ITextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.StartIndex + context.IndexOffset;
        }

        public static bool IsEnd(this ITextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.StartIndex + context.IndexOffset == context.Text.Length;
        }

        public static char GetCurrentChar(this ITextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return context.Text[context.GetIndex()];
        }

        public static char? TryGetNextChar(this ITextProcessingContext context)
        {
            var index = context.GetIndex();
            var len = context.Text.Length;

            var desiredIndex = index + 1;

            if (desiredIndex > len)
            {
                throw new InvalidOperationException();
            }
            else if (desiredIndex == len)
            {
                return null;
            }

            return context.Text[desiredIndex];
        }

        public static char? TryGetPreviousChar(this ITextProcessingContext context)
        {
            var index = context.GetIndex();
            var desiredIndex = index - 1;

            if (desiredIndex == -1)
            {
                return null;
            }

            if (desiredIndex >= context.Text.Length)
            {
                throw new InvalidOperationException();
            }

            return context.Text[desiredIndex];
        }

        public static char GetPreviousChar(this ITextProcessingContext context)
        {
            var c = context.TryGetPreviousChar();
            if (c.HasValue)
            {
                return c.Value;
            }
            throw new InvalidOperationException();
        }

        public static char GetCharAtOffset(this ITextProcessingContext context, int offset)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            var startIndex = context.StartIndex;
            var desiredIndex = startIndex + offset;
            if (desiredIndex >= context.Text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            return context.Text[desiredIndex];
        }

        public static Position GetCurrentPosition(this ITextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new Position(context.Line, context.Column);
        }

        public static void AdvanceByChar(this ITextProcessingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Advance(1, 0, context.Column + 1);
        }

        public static void AdvanceByResult(this ITextProcessingContext context, TextProcessingResult result)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());
        }
    }
}
