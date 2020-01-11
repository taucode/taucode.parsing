using System;
using TauCode.Extensions;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lab.TextDecorations;
using TauCode.Parsing.Lab.Tokens;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class PathExtractor : GammaTokenExtractorBase<TextTokenLab>
    {
        public override TextTokenLab ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var str = text.Substring(absoluteIndex, consumedLength);

            var token = new TextTokenLab(
                PathTextClass.Instance,
                NoneTextDecorationLab.Instance,
                str,
                position,
                consumedLength);

            return token;

        }

        protected override void OnBeforeProcess()
        {
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
