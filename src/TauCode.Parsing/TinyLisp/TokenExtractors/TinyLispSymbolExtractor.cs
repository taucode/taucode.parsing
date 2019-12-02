using TauCode.Parsing.Lexizing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispSymbolExtractor : TokenExtractorBase
    {
        public TinyLispSymbolExtractor() 
            : base(
                TinyLispHelper.IsSpace,
                TinyLispHelper.IsLineBreak,
                TinyLispHelper.IsAcceptableSymbolNameChar)
        {
        }

        protected override void Reset()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();

            if (int.TryParse(str, out var dummy)) // todo: move this check to helper.
            {
                return null;
            }

            return new LispSymbolToken(str);
        }

        protected override CharChallengeResult TestCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (c.IsAcceptableSymbolNameChar())
            {
                return CharChallengeResult.Continue; // todo: deal with integers, doubles and ratios.
            }

            return CharChallengeResult.Finish;
        }

        protected override bool TestEnd() => true;
    }
}
