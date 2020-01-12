using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing.Processors
{
    public class SpaceSkipper : TextProcessorBase
    {
        public override bool AcceptsFirstChar(char c) => LexingHelper.IsInlineWhiteSpace(c);

        public override TextProcessingResult Process(ITextProcessingContext context)
        {
            context.RequestGeneration();

            while (true)
            {
                if (context.IsEnd())
                {
                    break;
                }

                var c = context.GetCurrentChar();
                if (LexingHelper.IsInlineWhiteSpace(c))
                {
                    // go on.
                    context.AdvanceByChar();
                }
                else
                {
                    break;
                }
            }

            context.ReleaseGenerationAndGetMetrics(out var indexShift, out var lineShift, out var currentColumn);
            return new TextProcessingResult(indexShift, lineShift, currentColumn, EmptyPayload.Value);
        }
    }
}
