using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispCommentExtractor : TokenExtractorBase
    {
        public TinyLispCommentExtractor()
            : base(
                StandardLexingEnvironment.Instance,
                x => x == ';')
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            return new CommentToken(str);
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override CharChallengeResult ChallengeCurrentChar()
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
                    throw LexingHelper.CreateInternalErrorException();
                }
            }

            if (this.Environment.IsLineBreak(c))
            {
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.Continue;
        }

        protected override CharChallengeResult ChallengeEnd() =>
            CharChallengeResult.Finish; // LISP comment can be terminated by the end of input, no problem.
    }
}
