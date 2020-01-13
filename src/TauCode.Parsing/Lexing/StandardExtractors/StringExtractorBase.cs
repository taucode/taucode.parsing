using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardExtractors
{
    public class StringExtractorBase : TokenExtractorBase<TextToken>
    {
        private IList<EscapeProcessorBase> _escapeProcessors;

        public StringExtractorBase(
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

        public StringExtractorBase(
            char openingDelimiter,
            char closingDelimiter,
            bool acceptsNewLine,
            ITextDecoration textDecoration,
            EscapeProcessorBase[] escapeProcessors,
            Type[] acceptablePreviousTokenTypes)
            : this(
                openingDelimiter,
                closingDelimiter,
                acceptsNewLine,
                textDecoration,
                acceptablePreviousTokenTypes)
        {
            _escapeProcessors = CheckEscapeProcessors(escapeProcessors);
        }

        protected char OpeningDelimiter { get; }
        protected char ClosingDelimiter { get; }
        protected bool AcceptsNewLine { get; }
        protected ITextDecoration TextDecoration { get; }

        private static IList<EscapeProcessorBase> CheckEscapeProcessors(IList<EscapeProcessorBase> escapeProcessors)
        {
            if (escapeProcessors == null)
            {
                throw new ArgumentNullException(nameof(escapeProcessors));
            }

            if (escapeProcessors.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(escapeProcessors)}' cannot contain nulls.");
            }

            return escapeProcessors;
        }

        protected IList<EscapeProcessorBase> EscapeProcessors =>
            _escapeProcessors ?? (_escapeProcessors = CheckEscapeProcessors(this.CreateEscapeProcessors()));

        protected StringBuilder StringBuilder { get; private set; }

        protected virtual IList<EscapeProcessorBase> CreateEscapeProcessors()
        {
            return new List<EscapeProcessorBase>();
        }

        protected override TextProcessingResult SubProcess()
        {
            var gotEscape = false;

            foreach (var escapeProcessor in this.EscapeProcessors)
            {
                if (escapeProcessor.AcceptsFirstChar(this.Context.GetCurrentChar()))
                {
                    gotEscape = true;
                    var subResult = escapeProcessor.Process(this.Context);
                    if (subResult.IsSuccessful())
                    {
                        return subResult;
                    }
                }
            }

            if (gotEscape)
            {
                throw new LexingException("Wrong string escaping", this.Context.GetCurrentPosition());
            }

            return TextProcessingResult.Failure;
        }

        protected override bool ProcessEnd()
        {
            throw LexingHelper.CreateUnclosedStringLexingException(this.Context.GetCurrentPosition());
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
                    throw new LexingException("Newline in string constant", this.Context.GetCurrentPosition());
                }
            }

            this.StringBuilder.Append(c);
            return CharAcceptanceResult.Continue;

        }

        protected override void ConsumeSubPayload(IPayload subPayload)
        {
            var escapePayload = (EscapePayload)subPayload;
            this.StringBuilder.Append(escapePayload);
        }

        protected override void OnBeforeProcess()
        {
            this.StringBuilder = new StringBuilder();
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var realText = this.StringBuilder.ToString();
            this.StringBuilder = null;
            return new TextToken(StringTextClass.Instance, this.TextDecoration, realText, position, consumedLength);
        }
    }
}
