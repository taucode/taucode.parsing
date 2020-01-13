using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing.StandardEscapeProcessors
{
    public class CrEscapeProcessor : EscapeProcessorBase
    {
        public CrEscapeProcessor()
            : base(LexingHelper.CR, true)
        {
        }

        public override void AdvanceByEscapeChar()
        {
            this.Context.Advance(1, 1, 0);
        }

        protected override EscapePayload DeliverPayloadImpl()
        {
            if (this.Context.IsEnd())
            {
                // actually, there is an error, since string is unclosed,
                // but it's none of my business. StringExtractor will deal with it.
                return new EscapePayload(LexingHelper.CR, null);
            }

            if (this.Context.GetCurrentChar() == LexingHelper.LF)
            {
                this.Context.Advance(1, 0, 0); // line already shifted by 'AdvanceByEscapeChar', and column is still 0.
                return new EscapePayload(LexingHelper.CR, LexingHelper.LF);
            }

            return new EscapePayload(LexingHelper.CR, null);
        }
    }
}