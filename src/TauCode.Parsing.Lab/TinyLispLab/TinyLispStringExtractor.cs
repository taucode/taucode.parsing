using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Lab.TinyLispLab
{
    public class TinyLispStringExtractor : GammaTokenExtractorBase<OldTextToken>
    {
        private char _openingDelimiter;

        public override OldTextToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var str = text.Substring(absoluteIndex + 1, consumedLength - 2);
            return new OldTextToken(
                OldStringTextClass.Instance,
                OldDoubleQuoteTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            // todo: temporary check that IsProcessing == FALSE, everywhere
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            // todo: temporary check that LocalPosition == 1, everywhere
            if (this.Context.GetLocalIndex() != 1)
            {
                throw new NotImplementedException();
            }

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
