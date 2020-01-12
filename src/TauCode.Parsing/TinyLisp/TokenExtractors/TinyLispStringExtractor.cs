using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispStringExtractor : TokenExtractorBase<TextToken>
    {
        private char _openingDelimiter;

        public override TextToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new TextToken(
                StringTextClass.Instance,
                DoubleQuoteTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            _openingDelimiter = this.Context.GetLocalChar(0);
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            throw new NotImplementedException();
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return ContinueOrFail(c == '"');
            }

            if (c == '"')
            {
                this.Context.AdvanceByChar();
                return CharAcceptanceResult.Stop;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                throw new LexingException("Newline in string.", this.Context.GetCurrentAbsolutePosition());
            }

            return CharAcceptanceResult.Continue;
        }

        protected override bool ProcessEnd()
        {
            throw new LexingException("Unclosed string.", this.StartPosition);
        }
    }
}
