using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.Old.TinyLisp.TokenExtractors
{
    public class OldTinyLispSymbolExtractor : OldTokenExtractorBase
    {
        public OldTinyLispSymbolExtractor() 
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

            var position = new Position(this.StartLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;

            return new LispSymbolToken(str, position, consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (c.IsAcceptableSymbolNameChar())
            {
                return OldCharChallengeResult.Continue;
            }

            return OldCharChallengeResult.Finish;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            return OldCharChallengeResult.Finish; // symbol ends with end-of-input? no problem then.
        }
    }
}
