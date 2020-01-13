using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class TermExtractor : TokenExtractorBase<TextToken>
    {
        public TermExtractor()
            : base(null)
        {
        }

        protected override void OnBeforeProcess()
        {
            // idle
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex, consumedLength);
            return new TextToken(TermTextClass.Instance, NoneTextDecoration.Instance, str, position, consumedLength);
        }

        protected override bool ProcessEnd()
        {
            return this.Context.GetPreviousChar() != '-';
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(
                    LexingHelper.IsDigit(c) ||
                    LexingHelper.IsLatinLetter(c));
            }

            if (c == '-')
            {
                if (this.Context.GetPreviousChar() == '-')
                {
                    return CharAcceptanceResult.Fail;
                }

                return CharAcceptanceResult.Continue;
            }

            if (LexingHelper.IsDigit(c) ||
                LexingHelper.IsLatinLetter(c))
            {
                return CharAcceptanceResult.Continue;
            }

            if (c == '=' || LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Fail;
        }
    }
}
