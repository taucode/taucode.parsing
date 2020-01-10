namespace TauCode.Parsing.Lab
{
    public static class TextProcessingContextExtensions
    {
        public static void ReleaseGenerationAndGetMetrics(
            this TextProcessingContext context,
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
    }
}
