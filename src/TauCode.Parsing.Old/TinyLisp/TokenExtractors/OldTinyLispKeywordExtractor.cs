using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.Old.TinyLisp.TokenExtractors
{
    public class OldTinyLispKeywordExtractor : OldTokenExtractorBase
    {
        public OldTinyLispKeywordExtractor()
            : base(x => x == ':')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var res = this.ExtractResultString();

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new KeywordToken(res, position, consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return OldCharChallengeResult.Continue; // 0th char is always ok
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return OldCharChallengeResult.Continue;
            }

            return OldCharChallengeResult.Finish;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            if (this.LocalCharIndex > 1)
            {
                // consumed more than one char (0th is always ':'), so no problem here
                return OldCharChallengeResult.Finish;
            }
            else
            {
                throw new LexingException("Invalid symbol name.", this.GetCurrentAbsolutePosition());
            }
        }
    }
}
