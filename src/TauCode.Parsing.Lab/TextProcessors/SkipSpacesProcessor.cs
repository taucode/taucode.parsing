using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lab.TextProcessors
{
    // todo clean up
    public class SkipSpacesProcessor : ITextProcessor<string>
    {
        private bool _isProcessing;
        public bool AcceptsFirstChar(char c) => LexingHelper.IsInlineWhiteSpace(c);

        // todo: copy-pasted a lot.
        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                if (value == _isProcessing)
                {
                    throw new NotImplementedException(); // todo suspicious: why set to same value?
                }

                _isProcessing = value;
            }
        }


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
