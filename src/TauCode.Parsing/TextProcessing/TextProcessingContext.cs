using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.TextProcessing
{
    public class TextProcessingContext : ITextProcessingContext
    {
        #region Nested

        private class Generation
        {
            private readonly TextProcessingContext _holder;

            public Generation(TextProcessingContext holder)
            {
                _holder = holder;

                this.StartIndex = holder.GetAbsoluteIndex();
                this.LocalIndex = 0;
                this.CurrentLine = holder.GetCurrentLine();
                this.CurrentColumn = holder.GetCurrentColumn();
            }

            public int StartIndex { get; private set; }
            public int LocalIndex { get; private set; }
            public int CurrentLine { get; private set; }
            public int CurrentColumn { get; private set; }

            public int GetAbsoluteIndex() => this.StartIndex + this.LocalIndex;

            public void Advance(int indexShift, int lineShift, int currentColumn)
            {
                this.LocalIndex += indexShift;
                this.CurrentLine += lineShift;
                this.CurrentColumn = currentColumn;

                _holder._version++;
            }

            public Position GetCurrentAbsolutePosition()
            {
                return new Position(this.CurrentLine, this.CurrentColumn);
            }
        }

        #endregion

        #region Fields

        private readonly Stack<Generation> _generations;
        private int _version;

        #endregion

        #region Constructor

        public TextProcessingContext(string text)
        {
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
            _generations = new Stack<Generation>();
            var rootGeneration = new Generation(this);
            _generations.Push(rootGeneration);
            _version = 1;
        }

        #endregion

        #region Private

        private Generation GetLastGeneration()
        {
            if (_generations.Count == 0)
            {
                return null;
            }

            return _generations.Peek();
        }

        #endregion

        #region ITextProcessingContext Members

        public string Text { get; }

        public void RequestGeneration()
        {
            var generation = new Generation(this);
            _generations.Push(generation);
        }

        public void ReleaseGeneration()
        {
            if (this.Depth == 1)
            {
                throw LexingHelper.CreateInternalErrorLexingException(
                    null,
                    "Generation release was requested at depth 1.");
            }

            _generations.Pop();
        }

        public int Depth => _generations.Count;

        public int Version => _version;

        public int GetCurrentLine() => this.GetLastGeneration()?.CurrentLine ?? 0;

        public int GetAbsoluteIndex() => this.GetLastGeneration()?.GetAbsoluteIndex() ?? 0;

        public Position GetCurrentAbsolutePosition() => this.GetLastGeneration()?.GetCurrentAbsolutePosition() ?? Position.Zero;

        public int GetCurrentColumn() => this.GetLastGeneration()?.CurrentColumn ?? 0;

        public int GetStartIndex() => this.GetLastGeneration()?.StartIndex ?? 0;

        public int GetLocalIndex()
        {
            var lastGeneration = _generations.Peek();
            var localIndex = lastGeneration.LocalIndex;

            return localIndex;
        }

        public bool IsEnd()
        {
            var lastGeneration = _generations.Peek();
            var absoluteIndex = lastGeneration.StartIndex + lastGeneration.LocalIndex;
            if (absoluteIndex > this.Text.Length)
            {
                throw LexingHelper.CreateInternalErrorLexingException(
                    null,
                    $"{nameof(ITextProcessingContext)} is in an invalid state.");
            }

            return absoluteIndex == this.Text.Length;
        }

        public void Advance(int indexShift, int lineShift, int currentColumn)
        {
            if (indexShift <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(indexShift));
            }

            if (lineShift < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lineShift));
            }

            if (currentColumn < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentColumn));
            }

            this.GetLastGeneration().Advance(indexShift, lineShift, currentColumn);
        }

        public char GetCurrentChar()
        {
            // todo checks
            // todo should be extension
            var absoluteIndex = this.GetAbsoluteIndex();
            return this.Text[absoluteIndex];
        }

        public char GetLocalChar(int localIndex)
        {
            // todo checks
            // todo should be extension
            var absoluteIndex = this.GetStartIndex() + localIndex;
            return this.Text[absoluteIndex];
        }

        public char? TryGetNextLocalChar()
        {
            if (this.IsEnd())
            {
                // todo copy/pasted
                throw LexingHelper.CreateInternalErrorLexingException(
                    null,
                    $"{nameof(ITextProcessingContext)} is in an invalid state.");

            }

            var wantedIndex = this.GetAbsoluteIndex() + 1;
            if (wantedIndex == this.Text.Length)
            {
                return null;
            }

            return this.Text[wantedIndex];
        }

        public char? TryGetPreviousLocalChar()
        {
            if (this.GetLocalIndex() == 0)
            {
                return null;
            }

            var absoluteIndex = this.GetAbsoluteIndex();
            var wantedIndex = absoluteIndex - 1;
            return this.Text[wantedIndex];
        }

        #endregion
    }
}
