using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.TinyLisp.TokenExtractors
{
    public class OldTinyLispCommentExtractor : OldTokenExtractorBase
    {
        public OldTinyLispCommentExtractor()
            : base(x => x == ';')
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;
            return new OldCommentToken(str, position, consumedLength);
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (this.LocalCharIndex == 0)
            {
                // 0th char is always accepted by 'firstCharPredicate'
                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                this.SkipSingleLineBreak();
                return OldCharChallengeResult.Finish;
            }

            return OldCharChallengeResult.Continue; // comment is going on.
        }

        protected override OldCharChallengeResult ChallengeEnd() =>
            OldCharChallengeResult.Finish; // LISP comment can be terminated by the end of input, no problem.
    }
}
