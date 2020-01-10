using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

// todo clean
namespace TauCode.Parsing.Lab.TinyLispLab
{
    public class TinyLispPunctuationExtractorLab : GammaTokenExtractorBase<LispPunctuationToken>
    {
        public override LispPunctuationToken ProduceToken(string text, int absoluteIndex, int consumedLength,
            Position position)
        {
            return new LispPunctuationToken(
                TinyLispHelper.CharToPunctuation(text[absoluteIndex]),
                position,
                consumedLength);
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return
                previousToken is LispPunctuationToken ||
                previousToken is IntegerToken ||
                previousToken is KeywordToken ||
                previousToken is LispSymbolToken ||
                previousToken is TextToken;
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                var isPunctuation = TinyLispHelper.IsPunctuation(c);
                if (isPunctuation)
                {
                    return CharAcceptanceResult.Continue;
                }
                else
                {
                    return CharAcceptanceResult.Fail;
                }
            }

            // todo: check localIndex == 1?

            return CharAcceptanceResult.Stop;
        }
    }
}
