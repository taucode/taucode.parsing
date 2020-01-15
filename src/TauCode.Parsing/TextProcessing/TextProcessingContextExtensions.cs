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

// todo: IsEndIfAdvance(int advance)
        public static bool IsEndAtOffset(this ITextProcessingContext context, int offset)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var len = context.Text.Length;

            var indexToTest = context.StartIndex + context.IndexOffset + offset;
            if (indexToTest > len)
            {
                throw new InvalidOperationException();
            }
            else if (indexToTest == len)
            {
                return true;
            }

            return false;

        }

        public static bool IsEnd(this ITextProcessingContext context) => context.IsEndAtOffset(0);

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

        public static void AdvanceByChars(this ITextProcessingContext context, int shift)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Advance(shift, 0, context.Column + shift);
        }

        //public static void AdvanceByResult(this ITextProcessingContext context, TextProcessingResult result)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException(nameof(context));
        //    }

        //    context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());
        //}

        public static bool RequestChars(this ITextProcessingContext context, int count)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var startIndex = context.StartIndex;
            var indexOffset = context.IndexOffset;
            var len = context.Text.Length;
            var absoluteIndex = startIndex + indexOffset;

            if (absoluteIndex >= len)
            {
                throw new InvalidOperationException();
            }

            var remaining = len - absoluteIndex;
            return remaining >= count;
        }

        public static string GetSubstring(this ITextProcessingContext context, int length)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            var absoluteIndex = context.StartIndex + context.IndexOffset;
            var totalLength = context.Text.Length;

            if (absoluteIndex >= totalLength)
            {
                throw new InvalidOperationException();
            }

            return context.Text.Substring(absoluteIndex, length);
        }

// todo: GetCurrentRemainder. also add GetRemainderAtOffset
        //public static string GetRemainder(this ITextProcessingContext context)
        //{
        //    if (context == null)
        //    {
        //        throw new ArgumentNullException(nameof(context));
        //    }

        //    var index = context.StartIndex + context.IndexOffset;
        //    var textLength = context.Text.Length;
        //    if (index > textLength)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    return context.Text.Substring(index);
        //}
    }
}
