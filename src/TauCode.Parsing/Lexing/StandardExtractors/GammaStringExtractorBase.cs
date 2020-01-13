using System;
using System.Collections.Generic;
using System.Text;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class GammaStringExtractorBase : TokenExtractorBase<TextToken>
    {
        private IList<ITextProcessor> _escapeProcessors;

        public GammaStringExtractorBase(
            char openingDelimiter,
            char closingDelimiter,
            bool acceptsNewLine,
            ITextDecoration textDecoration,
            Type[] acceptablePreviousTokenTypes)
            : base(acceptablePreviousTokenTypes)
        {
            this.OpeningDelimiter = openingDelimiter;
            this.ClosingDelimiter = closingDelimiter;
            this.AcceptsNewLine = acceptsNewLine;
            this.TextDecoration = textDecoration ?? throw new ArgumentNullException(nameof(textDecoration));
        }

        protected char OpeningDelimiter { get; }
        protected char ClosingDelimiter { get; }
        protected bool AcceptsNewLine { get; }
        protected ITextDecoration TextDecoration { get; }

        protected IList<ITextProcessor> EscapeProcessors =>
            _escapeProcessors ?? (_escapeProcessors = this.CreateEscapeProcessors());

        protected StringBuilder StringBuilder { get; private set; }

        protected override TextProcessingResult SubProcess()
        {
            foreach (var escapeProcessor in this.EscapeProcessors)
            {
                if (escapeProcessor.AcceptsFirstChar(this.Context.GetCurrentChar()))
                {
                    var subResult = escapeProcessor.Process(this.Context);
                    if (subResult.IsSuccessful())
                    {
                        return subResult;
                    }
                }
            }

            return TextProcessingResult.Failure;
        }

        protected override bool ProcessEnd()
        {
            throw new NotImplementedException(); // unclosed string!
        }

        protected virtual IList<ITextProcessor> CreateEscapeProcessors()
        {
            return new List<ITextProcessor>();
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return this.ContinueOrFail(c == this.OpeningDelimiter);
            }

            if (c == this.ClosingDelimiter)
            {
                this.Context.AdvanceByChar();
                return CharAcceptanceResult.Stop;
            }

            if (LexingHelper.IsCaretControl(c))
            {
                if (this.AcceptsNewLine)
                {
                    this.StringBuilder.Append(c);
                    return CharAcceptanceResult.Continue;
                }
                else
                {
                    throw new NotImplementedException(); // new line in string constant.
                }
            }

            this.StringBuilder.Append(c);
            return CharAcceptanceResult.Continue;
        }

        protected override void OnBeforeProcess()
        {
            StringBuilder = new StringBuilder();
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var realText = this.StringBuilder.ToString();
            this.StringBuilder = null;
            return new TextToken(StringTextClass.Instance, this.TextDecoration, realText, position, consumedLength);
        }
    }
}
