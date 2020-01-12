using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispKeywordExtractor : TokenExtractorBase<KeywordToken>
    {
        public TinyLispKeywordExtractor()

            : base(new[]
            {
                typeof(LispPunctuationToken)
            })
        {
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            // idle
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                return this.ContinueOrFail(c == ':');
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return CharAcceptanceResult.Continue;
            }

            return CharAcceptanceResult.Stop;
        }

        protected override KeywordToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var keyword = text.Substring(absoluteIndex, consumedLength);
            return new KeywordToken(keyword, position, consumedLength);
        }
    }
}
