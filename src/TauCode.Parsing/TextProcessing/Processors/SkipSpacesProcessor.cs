using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing.Processors
{
    public class SkipSpacesProcessor : TextProcessorBase<string> // todo: Nothing.
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
            return new TextProcessingResult(TextProcessingSummary.Skip, indexShift, lineShift, currentColumn);
        }

        public override string Produce(string text, int absoluteIndex, Position position, int consumedLength)
        {
            throw new NotImplementedException(); // todo should never be called
        }
    }
}
