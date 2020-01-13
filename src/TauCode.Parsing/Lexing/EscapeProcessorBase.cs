using System;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public abstract class EscapeProcessorBase : TextProcessorBase
    {
        protected EscapeProcessorBase(char escapeChar, bool allowsSingleChar)
        {
            this.EscapeChar = escapeChar;
            this.AllowsSingleChar = allowsSingleChar;
        }

        protected char EscapeChar { get; }

        protected bool AllowsSingleChar { get; }

        protected ITextProcessingContext Context { get; private set; }

        public override bool AcceptsFirstChar(char c) => c == this.EscapeChar;

        public virtual void AdvanceByEscapeChar()
        {
            this.Context.AdvanceByChar();
        }

        public override TextProcessingResult Process(ITextProcessingContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            
            this.Context.RequestGeneration();
            this.AdvanceByEscapeChar();

            if (this.Context.IsEnd() && !this.AllowsSingleChar)
            {
                throw LexingHelper.CreateUnclosedStringLexingException(this.Context.GetCurrentPosition());
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
