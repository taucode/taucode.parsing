using TauCode.Extensions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing.Processors
{
    public class NewLineSkipper : TextProcessorBase
    {
        public NewLineSkipper(bool wantsSingleResult)
        {
            this.WantsSingleResult = wantsSingleResult;
        }

        public bool WantsSingleResult { get; }

        public override bool AcceptsFirstChar(char c) => c.IsIn(
            LexingHelper.CR,
            LexingHelper.LF);

        public override TextProcessingResult Process(ITextProcessingContext context)
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
                    case LexingHelper.CR:
                        if (this.WantsSingleResult)
                        {
                            // whatever outcome is, stop processing.
                            goOn = false;
                        }

                        var nextChar = context.TryGetNextChar();
                        if (nextChar.HasValue)
                        {
                            if (nextChar.Value == LexingHelper.LF)
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

                    case LexingHelper.LF:
                        if (this.WantsSingleResult)
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
            return new TextProcessingResult(indexShift, lineShift, currentColumn, EmptyPayload.Value);
        }
    }
}
