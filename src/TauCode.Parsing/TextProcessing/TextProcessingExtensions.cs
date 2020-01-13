using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing
{
    public static class TextProcessingExtensions
    {
        public static bool IsSuccessful(this TextProcessingResult textProcessingResult) =>
            textProcessingResult.IndexShift > 0;

        public static int GetCurrentColumn(this TextProcessingResult result) =>
            result.CurrentColumn ??
            throw LexingHelper.CreateInternalErrorLexingException(null, "Invalid operation: result doesn't have current column.");
    }
}
