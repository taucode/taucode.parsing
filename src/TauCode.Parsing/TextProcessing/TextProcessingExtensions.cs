using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing
{
    public static class TextProcessingExtensions // todo merge all extensions into one static class?
    {
        public static void AlphaCheckNotBusyAndContextIsNull(this ITextProcessor textProcessor)
        {
            if (textProcessor.IsBusy || textProcessor.AlphaGetContext() != null)
            {
                throw LexingHelper.CreateInternalErrorLexingException(null, "Text processor is busy while it should not.");
            }
        }

        public static void AlphaCheckDepthOne(this ITextProcessingContext context)
        {
            if (context.Depth != 1)
            {
                throw new AlphaException("Expected depth 1.");
            }
        }

        public static bool IsSuccessful(this TextProcessingResult textProcessingResult) =>
            textProcessingResult.IndexShift > 0;

        public static int GetCurrentColumn(this TextProcessingResult result) =>
            result.CurrentColumn ??
            throw LexingHelper.CreateInternalErrorLexingException(null, "Invalid operation: result doesn't have current column.");
    }
}
