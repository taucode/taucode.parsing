using System;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class KeyExtractor : GammaTokenExtractorBase<OldTextToken>
    {
        private int _hyphenCountInARow;

        public override OldTextToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var str = text.Substring(absoluteIndex, consumedLength);
            return new OldTextToken(KeyTextClass.Instance, OldNoneTextDecoration.Instance, str, position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            _hyphenCountInARow = 1; // guaranteed to start with '-'.
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            throw new NotImplementedException();
        }

        protected override bool ProcessEnd()
        {
            return this.Context.GetPreviousAbsoluteChar().Value != '-';
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

            // todo: this is bad. you should not give a shit about 'GetPreviousAbsoluteChar'. work with local chars only. here & anywhere.
            var previousChar = this.Context.GetPreviousAbsoluteChar().Value;

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
