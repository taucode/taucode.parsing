using System;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class StringExtractor : TokenExtractorBase<TextToken>
    {
        private char _openingDelimiter;

        public StringExtractor(params Type[] acceptablePreviousTokenTypes)
            : base(acceptablePreviousTokenTypes)
        {
        }

        private static ITextDecoration GetDecoration(char openingDelimiter, Position position)
        {
            switch (openingDelimiter)
            {
                case '"':
                    return DoubleQuoteTextDecoration.Instance;

                case '\'':
                    return SingleQuoteTextDecoration.Instance;

                default:
                    throw new LexingException($"Invalid string delimiter: '{openingDelimiter}'", position);
            }
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            _openingDelimiter = this.Context.GetCharAtOffset(0);
        }

        protected override bool ProcessEnd()
        {
            throw new LexingException("Unclosed string.", this.Context.GetCurrentPosition());
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                return this.ContinueOrFail(c.IsIn('\'', '"'));
            }

            if (c == _openingDelimiter)
            {
                this.Context.AdvanceByChar();
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Continue;
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new TextToken(
                StringTextClass.Instance,
                GetDecoration(_openingDelimiter, this.StartPosition),
                str,
                position,
                consumedLength);
        }
    }
}
