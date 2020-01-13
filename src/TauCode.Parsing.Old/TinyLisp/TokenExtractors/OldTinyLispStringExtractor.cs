using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.TinyLisp.TokenExtractors
{
    public class OldTinyLispStringExtractor : OldTokenExtractorBase
    {
        public OldTinyLispStringExtractor()
            : base(x => x == '"')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);

            var position = new Position(this.StartLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;

            return new OldTextToken(
                OldStringTextClass.Instance,
                OldDoubleQuoteTextDecoration.Instance,
                value,
                position,
                consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                throw new LexingException("Newline in string.", this.GetCurrentAbsolutePosition());
            }

            if (c == '"')
            {
                this.Advance();
                return OldCharChallengeResult.Finish;
            }

            return OldCharChallengeResult.Continue;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            throw new LexingException("Non-closed string.", this.GetStartAbsolutePosition());
        }
    }
}
