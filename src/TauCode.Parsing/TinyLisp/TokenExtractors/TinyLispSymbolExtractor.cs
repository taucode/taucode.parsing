using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispSymbolExtractor : TokenExtractorBase
    {
        public TinyLispSymbolExtractor() 
            : base(
                //StandardLexingEnvironment.Instance,
                TinyLispHelper.IsAcceptableSymbolNameChar)
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();

            if (int.TryParse(str, out var dummy))
            {
                return null;
            }

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new LispSymbolToken(str, position, consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (c.IsAcceptableSymbolNameChar())
            {
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            return CharChallengeResult.Finish; // symbol ends with end-of-input? no problem then.
        }
    }
}
