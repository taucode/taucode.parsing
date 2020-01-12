using System;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class StringExtractor : TokenExtractorBase<TextToken>
    {
        private char _openingDelimiter;

        public override TextToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new TextToken(
                StringTextClass.Instance,
                GetDecoration(_openingDelimiter),
                str,
                position,
                consumedLength);
        }

        private static ITextDecoration GetDecoration(char openingDelimiter)
        {
            switch (openingDelimiter)
            {
                case '"':
                    return DoubleQuoteTextDecoration.Instance;

                case '\'':
                    return SingleQuoteTextDecoration.Instance;

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
