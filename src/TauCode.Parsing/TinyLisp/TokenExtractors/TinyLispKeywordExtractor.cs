using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispKeywordExtractor : TokenExtractorBase<KeywordToken>
    {
        public override KeywordToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var keyword = text.Substring(absoluteIndex, consumedLength);
            return new KeywordToken(keyword, position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();
            // idle
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return previousToken is LispPunctuationToken;
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c == ':');
            }

            var isMine = c.IsAcceptableSymbolNameChar();
            if (isMine)
            {
                return CharAcceptanceResult.Continue;
            }

            return CharAcceptanceResult.Stop;
        }
    }
}
