using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispSymbolExtractor : TokenExtractorBase<LispSymbolToken>
    {
        public TinyLispSymbolExtractor()
            : base(new[]
            {
                typeof(LispPunctuationToken)
            })
        {
        }

        protected override void OnBeforeProcess()
        {
            // idle
        }

        protected override LispSymbolToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var symbolString = text.Substring(absoluteIndex, consumedLength);
            if (int.TryParse(symbolString, out var dummy))
            {
                return null;
            }

            return new LispSymbolToken(symbolString, position, consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c.IsAcceptableSymbolNameChar());
            }

            if (c.IsAcceptableSymbolNameChar())
            {
                return CharAcceptanceResult.Continue;
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
