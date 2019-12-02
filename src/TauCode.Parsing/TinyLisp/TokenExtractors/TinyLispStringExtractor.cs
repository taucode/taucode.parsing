using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispStringExtractor : TokenExtractorBase
    {
        public TinyLispStringExtractor()
            : base(
                LexizingHelper.IsSpace,
                LexizingHelper.IsLineBreak,
                x => x == '"') // todo: consider extracting delegate into TinyLispHelper.
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var value = str.Substring(1, str.Length - 2);
            return new StringToken(value);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                if (c == '"')
                {
                    return CharChallengeResult.Continue;
                }
                else
                {
                    throw new LexerException("Internal error."); // how on earth we could even get here?
                }
            }

            if (c == '"')
            {
                this.Advance();
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.Continue;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            return CharChallengeResult.Error; // unclosed string. that's my error. no other extractor can handle this.
        }
    }
}
