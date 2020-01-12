using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispSymbolExtractorLab : GammaTokenExtractorBase<LispSymbolToken>
    {
        // todo: wtf! change places for <int consumedLength> and <Position position>
        public override LispSymbolToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var symbolString = text.Substring(absoluteIndex, consumedLength);
            if (int.TryParse(symbolString, out var dummy))
            {
                return null;
            }

            return new LispSymbolToken(symbolString, position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            // idle
        }

        // todo: Token-type-based default virtual implementation, for everybody.
        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return
                previousToken is LispPunctuationToken;
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                // todo: temp check that IsProcessing == false, everywhere.

                var accepts = c.IsAcceptableSymbolNameChar();
                if (accepts)
                {
                    return CharAcceptanceResult.Continue;
                }
                else
                {
                    return CharAcceptanceResult.Fail;
                }
            }

            if (c.IsAcceptableSymbolNameChar())
            {
                return CharAcceptanceResult.Continue;
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
