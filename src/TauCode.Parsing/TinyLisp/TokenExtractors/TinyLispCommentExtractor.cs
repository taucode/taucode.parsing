using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispCommentExtractor : TokenExtractorBase
    {
        public TinyLispCommentExtractor()
            : base(x => x == ';')
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;
            return new CommentToken(str, position, consumedLength);
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (this.LocalCharIndex == 0)
            {
                // 0th char is always accepted by 'firstCharPredicate'
                return CharChallengeResult.Continue;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                this.SkipSingleLineBreak();
                return CharChallengeResult.Finish;
            }

            return CharChallengeResult.Continue; // comment is going on.
        }

        protected override CharChallengeResult ChallengeEnd() =>
            CharChallengeResult.Finish; // LISP comment can be terminated by the end of input, no problem.
    }
}
