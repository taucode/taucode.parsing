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

        protected override void Reset()
        {
            //
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            return new CommentToken(str);
        }

        protected override TestCharResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return this.ContinueIf(c == ';');
            }

            if (this.IsLineBreakChar(c))
            {
                return TestCharResult.Finish;
            }

            return TestCharResult.Continue;
        }

        protected override bool TestEnd() => true;
    }
}
