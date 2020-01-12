using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class PathExtractor : TokenExtractorBase<TextToken>
    {
        public override TextToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex, consumedLength);

            var token = new TextToken(
                PathTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);

            return token;

        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            // idle
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            throw new NotImplementedException();
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(IsPathFirstChar(c));
            }

            if (IsPathFirstChar(c) || c == ':')
            {
                return CharAcceptanceResult.Continue;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Fail;
        }

        private static bool IsPathFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c) ||
            c.IsIn('\\', '/', '.', '!', '~', '$', '%', '-', '+');
    }
}
