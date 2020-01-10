using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextClasses;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Lab.TinyLispLab
{
    public class TinyLispStringExtractor : GammaTokenExtractorBase<TextToken>
    {
        public override TextToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new TextToken(
                StringTextClass.Instance,
                DoubleQuoteTextDecoration.Instance,
                str,
                position,
                consumedLength);
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
