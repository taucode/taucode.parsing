using System;

namespace TauCode.Parsing.TextProcessing
{
    public static class TextProcessingExtensions // todo merge all extensions into one static class?
    {
        public static void AlphaCheckNotBusy(this ITextProcessor textProcessor)
        {
            if (textProcessor.IsBusy)
            {
                throw new NotImplementedException();
            }
        }

        public static void AlphaCheckDepthOne(this ITextProcessingContext context)
        {
            if (context.Depth != 1)
            {
                throw new NotImplementedException();
            }
        }

        public static bool IsSuccessful(this TextProcessingResult textProcessingResult) =>
            textProcessingResult.IndexShift > 0;

        public static int GetCurrentColumn(this TextProcessingResult result) =>
            result.CurrentColumn ??
            throw new NotImplementedException(); // bad operation; something wrong with your logic.
    }
}
