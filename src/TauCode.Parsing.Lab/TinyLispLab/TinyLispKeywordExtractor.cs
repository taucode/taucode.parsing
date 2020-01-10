using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.Lab.TinyLispLab
{
    public class TinyLispKeywordExtractor : GammaTokenExtractorBase<KeywordToken>
    {
        public override KeywordToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var keyword = text.Substring(absoluteIndex, consumedLength);
            return new KeywordToken(keyword, position, consumedLength);
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
