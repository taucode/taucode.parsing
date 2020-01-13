using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class WordExtractor : TokenExtractorBase<TextToken>
    {
        public WordExtractor()
            : base(new[]
            {
                typeof(PunctuationToken),
            })
        {
        }

        protected override void OnBeforeProcess()
        {
            // idle
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = this.Context.Text.Substring(absoluteIndex, consumedLength);

            return new TextToken(
                WordTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c == '_' || LexingHelper.IsLatinLetter(c));
            }

            if (
                LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) ||
                LexingHelper.IsStandardPunctuationChar(c))
            {
                return CharAcceptanceResult.Stop;
            }

            if (c == '_' || LexingHelper.IsLatinLetter(c) || LexingHelper.IsDigit(c))
            {
                return CharAcceptanceResult.Continue;
            }

            // I don't want this char inside my word.
            return CharAcceptanceResult.Fail;
        }
    }
}
