using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TextProcessing.Processors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispCommentExtractor : TokenExtractorBase<CommentToken>
    {
        private readonly SkipLineBreaksProcessor _skipLineBreaksProcessor;

        public TinyLispCommentExtractor()
        {
            _skipLineBreaksProcessor = new SkipLineBreaksProcessor(true);
        }

        public override CommentToken ProduceToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            return new CommentToken(text.Substring(absoluteIndex, consumedLength), position, consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();
        }

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return true; // doesn't matter what previous token is.
        }

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
                    throw
                        new NotImplementedException(); // cannot be. todo: check it somewhere? (SkipperBase or something)
                }

                this.Context.Advance(skipLineBreaksResult.IndexShift, skipLineBreaksResult.LineShift,
                    skipLineBreaksResult.GetCurrentColumn());
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Continue; // collect any other chars into comment
        }
    }
}

