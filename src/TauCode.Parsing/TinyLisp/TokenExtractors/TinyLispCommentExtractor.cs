using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexizing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispCommentExtractor : TokenExtractorBase
    {
        public TinyLispCommentExtractor()
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak,
                x => x == ';') // todo: consider extracting delegate into TinyLispHelper.
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            return new CommentToken(str);
        }

        protected override CharChallengeResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                if (c == ';')
                {
                    return CharChallengeResult.Continue;
                }
                else
                {
                    throw new LexerException("Internal error."); // how on earth we could even get here?
                }
            }

            if (this.LineBreakPredicate(c))
            {
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.Continue;
        }

        protected override bool TestEnd() => true;
    }
}
