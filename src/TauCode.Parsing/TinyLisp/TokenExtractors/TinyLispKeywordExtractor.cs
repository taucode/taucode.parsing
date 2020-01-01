using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispKeywordExtractor : TokenExtractorBase
    {
        public TinyLispKeywordExtractor()
            : base(
                StandardLexingEnvironment.Instance,
                x => x == ':')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var res = this.ExtractResultString();

            return new KeywordToken(res);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                if (c == ':')
                {
                    return CharChallengeResult.Continue;
                }
                else
                {
                    throw LexingHelper.CreateInternalErrorException();
                }
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            if (this.GetLocalPosition() > 1)
            {
                // consumed more than one char (0th is always ':'), so no problem here
                return CharChallengeResult.Finish;
            }
            else
            {
                // consumed just one char (':'), therefore error. on one other token extractor in LISP can have ':' at the beginning.
                return CharChallengeResult.Error;
            }
        }
    }
}
