using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Lab.TextProcessors
{
    // todo clean up
    public class SkipLineBreaksProcessor : ITextProcessor<string>
    {
        private readonly bool _skipOnlyOneResult;

        public SkipLineBreaksProcessor(bool skipOnlyOneResult)
        {
            _skipOnlyOneResult = skipOnlyOneResult;
        }

        public bool AcceptsFirstChar(char c) => LexingHelper.IsCaretControl(c);

        public TextProcessingResult Process(TextProcessingContext context)
        {
            context.RequestGeneration();
            var goOn = true;

            while (goOn)
            {
                if (context.IsEnd())
                {
                    break;
                }

                var c = context.GetCurrentChar();

                switch (c)
                {
                    case LexingHelper.Cr:
                        if (_skipOnlyOneResult)
                        {
                            // whatever outcome is, stop processing.
                            goOn = false;
                        }

                        var nextChar = context.TryGetNextLocalChar();
                        if (nextChar.HasValue)
                        {
                            if (nextChar.Value == LexingHelper.Lf)
                            {
                                // got CRLF
                                context.Advance(2, 1, 0);
                                break;
                            }
                            else
                            {
                                // got CR and non-line-control-char
                                goOn = false; // redundant, but saves ~3 processor ticks :)
                                context.Advance(1, 1, 0);
                                break;
                            }
                        }
                        else
                        {
                            // end of input.
                            context.Advance(1, 1, 0);
                            break;
                        }

                    case '\n':
                        if (_skipOnlyOneResult)
                        {
                            // whatever outcome is, stop processing.
                            goOn = false;
                        }

                        context.Advance(1, 1, 0);
                        break;

                    default:
                        goOn = false;
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
