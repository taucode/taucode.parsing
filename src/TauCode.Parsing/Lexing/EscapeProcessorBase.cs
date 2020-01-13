using System;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public abstract class EscapeProcessorBase : TextProcessorBase
    {
        protected EscapeProcessorBase(char escapeChar)
        {
            this.EscapeChar = escapeChar;
        }

        protected char EscapeChar { get; }

        protected ITextProcessingContext Context { get; private set; }

        public override bool AcceptsFirstChar(char c) => c == this.EscapeChar;

        public override TextProcessingResult Process(ITextProcessingContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            
            this.Context.RequestGeneration();
            this.Context.AdvanceByChar(); // skip escape

            if (this.Context.IsEnd())
            {
                throw new NotImplementedException(); // unclosed escape
            }

            var payload = this.DeliverPayloadImpl();

            if (payload == null)
            {
                this.Context.ReleaseGeneration();
                return TextProcessingResult.Failure;
            }

            this.Context.ReleaseGenerationAndGetMetrics(out var indexShift, out var lineShift, out var currentColumn);

            return new TextProcessingResult(indexShift, lineShift, currentColumn, payload);
        }

        protected abstract EscapePayload DeliverPayloadImpl();
    }
}
