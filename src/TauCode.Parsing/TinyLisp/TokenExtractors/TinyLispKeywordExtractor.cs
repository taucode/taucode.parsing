using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispKeywordExtractor : TokenExtractorBase
    {
        public TinyLispKeywordExtractor()
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak,
                x => x == ':')
        {
        }

        protected override IToken ProduceResult()
        {
            var res = this.ExtractResultString();

            return new KeywordToken(res);
        }

        protected override CharChallengeResult TestCurrentChar()
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
                    throw new LexerException("Internal error."); // how on earth we could even get here?
                }
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish;
        }

        protected override bool TestEnd() => this.GetLocalPosition() > 1;
    }
}
