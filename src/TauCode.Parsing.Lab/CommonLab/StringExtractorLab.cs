using System;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Old;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Lab.CommonLab
{
    public class StringExtractorLab : GammaTokenExtractorBase<OldTextToken>
    {
        private char _openingDelimiter;

        public override OldTextToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new OldTextToken(
                OldStringTextClass.Instance,
                GetDecoration(_openingDelimiter),
                str,
                position,
                consumedLength);
        }

        private static IOldTextDecoration GetDecoration(char openingDelimiter)
        {
            switch (openingDelimiter)
            {
                case '"':
                    return OldDoubleQuoteTextDecoration.Instance;

                case '\'':
                    return OldSingleQuoteTextDecoration.Instance;

                default:
                    throw new NotImplementedException(); // error.
            }
        }

        protected override void OnBeforeProcess()
        {
            _openingDelimiter = this.Context.GetLocalChar(0);
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            throw new NotImplementedException();
        }

        protected override bool ProcessEnd()
        {
            throw new LexingException("Unclosed string.", this.Context.GetCurrentAbsolutePosition());
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c.IsIn('\'', '"'));
            }

            if (c == _openingDelimiter)
            {
                this.Context.AdvanceByChar();
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Continue;
        }
    }
}
