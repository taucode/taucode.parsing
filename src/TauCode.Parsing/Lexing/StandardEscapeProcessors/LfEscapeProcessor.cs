namespace TauCode.Parsing.Lexing.StandardEscapeProcessors
{
    public class LfEscapeProcessor : EscapeProcessorBase
    {
        public LfEscapeProcessor()
            : base(LexingHelper.LF, true)
        {
        }

        public override void AdvanceByEscapeChar()
        {
            this.Context.Advance(1, 1, 0);
        }

        protected override EscapePayload DeliverPayloadImpl()
        {
            // actually, there is an error, since string is unclosed,
            // but it's none of my business. StringExtractor will deal with it.
            return new EscapePayload(LexingHelper.LF, null);
        }
    }
}
