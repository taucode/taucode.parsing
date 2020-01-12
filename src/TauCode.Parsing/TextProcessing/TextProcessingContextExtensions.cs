using TauCode.Parsing.Exceptions;

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
            // todo checks on context's generations.

            var newIndex = context.GetAbsoluteIndex();
            var newLine = context.GetCurrentLine();
            currentColumn = context.GetCurrentColumn();

            context.ReleaseGeneration();

            var oldIndex = context.GetAbsoluteIndex();
            var oldLine = context.GetCurrentLine();

            indexShift = newIndex - oldIndex;
            lineShift = newLine - oldLine;
        }

        public static char GetPreviousLocalChar(this ITextProcessingContext context)
        {
            return context.TryGetPreviousLocalChar()
                   ??
                   throw new LexingException("Cannot get previous local char when local index is 0", context.GetCurrentAbsolutePosition());
        }

        public static void AdvanceByResult(this ITextProcessingContext context, TextProcessingResult result)
        {
            context.Advance(result.IndexShift, result.LineShift, result.GetCurrentColumn());
        }

    }
}
