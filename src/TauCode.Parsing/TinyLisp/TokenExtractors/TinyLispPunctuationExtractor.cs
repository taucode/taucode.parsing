using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispPunctuationExtractor : TokenExtractorBase<LispPunctuationToken>
    {
        public TinyLispPunctuationExtractor()
            : base(new[]
            {
                typeof(LispPunctuationToken),
                typeof(IntegerToken),
                typeof(KeywordToken),
                typeof(LispSymbolToken),
                typeof(TextToken),
            })
        {
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            // idle
        }

        protected override LispPunctuationToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            return new LispPunctuationToken(
                TinyLispHelper.CharToPunctuation(text[absoluteIndex]),
                position,
                consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(TinyLispHelper.IsPunctuation(c));
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
