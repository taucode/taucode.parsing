using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Lab.TextProcessors
{
    // todo clean up
    public class SkipSpacesProcessor : ITextProcessor<string>
    {
        public bool AcceptsFirstChar(char c) => LexingHelper.IsInlineWhiteSpace(c);

        public TextProcessingResult Process(ITextProcessingContext context)
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

        public string Produce(string text, int absoluteIndex, int consumedLength, Position position)
        {
            throw new NotImplementedException(); // todo should never be called
        }
    }
}
