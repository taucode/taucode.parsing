using System;
using TauCode.Parsing.Lab.TextProcessors;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab.TinyLispLab
{
    // todo clean
    public class TinyLispCommentExtractorLab : GammaTokenExtractorBase<CommentToken>
    {
        private readonly SkipLineBreaksProcessor _skipLineBreaksProcessor;

        public TinyLispCommentExtractorLab()
        {
            _skipLineBreaksProcessor = new SkipLineBreaksProcessor(true);
        }

        public override CommentToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            return new CommentToken(text.Substring(absoluteIndex, consumedLength), position, consumedLength);
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

            // idle
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return true; // doesn't matter what previous token is.
        }

        //protected override bool AcceptsPreviousCharImpl(char previousChar) => true; // accepts any previous char

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                if (c == ';')
                {
                    return CharAcceptanceResult.Continue;
                }

                return CharAcceptanceResult.Fail;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                var skipLineBreaksResult = _skipLineBreaksProcessor.Process(this.Context);
                if (skipLineBreaksResult.Summary != TextProcessingSummary.Skip)
                {
                    throw new NotImplementedException(); // cannot be. todo: check it somewhere? (SkipperBase or something)
                }

                this.Context.Advance(skipLineBreaksResult.IndexShift, skipLineBreaksResult.LineShift, skipLineBreaksResult.GetCurrentColumn());
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Continue; // collect any other chars into comment
        }
    }
}
