using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class KeyExtractor : TokenExtractorBase<TextToken>
    {
        private int _hyphenCountInARow;

        public override TextToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex, consumedLength);
            return new TextToken(KeyTextClass.Instance, NoneTextDecoration.Instance, str, position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            _hyphenCountInARow = 1; // guaranteed to start with '-'.
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            throw new NotImplementedException();
        }

        protected override bool ProcessEnd()
        {
            return this.Context.GetPreviousLocalChar() != '-';
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c == '-');
            }

            if (LexingHelper.IsDigit(c) ||
                LexingHelper.IsLatinLetter(c))
            {
                _hyphenCountInARow = 0;
                return CharAcceptanceResult.Continue;
            }

            var previousChar = this.Context.GetPreviousLocalChar();

            if (c == '-')
            {
                if (previousChar == '-')
                {
                    _hyphenCountInARow++;
                }

                if (_hyphenCountInARow > 2)
                {
                    return CharAcceptanceResult.Fail;
                }

                return CharAcceptanceResult.Continue;
            }


            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                if (previousChar == '-')
                {
                    return CharAcceptanceResult.Fail;
                }

                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Fail;
        }
    }
}
