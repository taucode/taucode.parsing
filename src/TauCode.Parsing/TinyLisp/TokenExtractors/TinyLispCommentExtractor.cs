using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispCommentExtractor : TokenExtractorBase<NullToken>
    {
        public TinyLispCommentExtractor()
            : base(null)
        {
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();
            // idle
        }

        protected override NullToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength) => 
            NullToken.Instance;

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return true; // doesn't matter what previous token is.
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                return this.ContinueOrFail(c == ';');
            }

            if (LexingHelper.IsCaretControl(c))
            {
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Continue; // collect any other chars into comment
        }
    }
}
