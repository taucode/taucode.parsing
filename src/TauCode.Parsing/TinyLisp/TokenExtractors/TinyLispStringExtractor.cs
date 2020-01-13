using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispStringExtractor : TokenExtractorBase<TextToken>
    {
        public TinyLispStringExtractor()
            : base(null) // todo: actually, accepts any.
        {

        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            // idle
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new TextToken(
                StringTextClass.Instance,
                DoubleQuoteTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                return ContinueOrFail(c == '"');
            }

            if (c == '"')
            {
                this.Context.AdvanceByChar();
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Continue;
        }

        protected override bool ProcessEnd()
        {
            throw new LexingException("Unclosed string.", this.StartPosition);
        }
    }
}
