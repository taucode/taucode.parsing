using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispStringExtractor : TokenExtractorBase
    {
        public TinyLispStringExtractor()
            : base(
                StandardLexingEnvironment.Instance,
                x => x == '"')
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
                    throw LexingHelper.CreateInternalErrorException();
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
