using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

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

        protected override bool AcceptsPreviousCharImpl(char previousChar)
        {
            return
                LexingHelper.IsInlineWhiteSpaceOrCaretControl(previousChar) ||
                TinyLispHelper.IsPunctuation(previousChar) ||
                TinyLispHelper.IsAcceptableSymbolNameChar(previousChar) ||
                previousChar == '"';
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
