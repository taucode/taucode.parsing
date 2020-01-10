using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Lab
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

                this.StartingIndex = holder.GetAbsoluteIndex();
                this.LocalIndex = 0;
                this.CurrentLine = holder.GetCurrentLine();
                this.CurrentColumn = holder.GetCurrentColumn();
            }

            public int StartingIndex { get; private set; }
            public int LocalIndex { get; private set; }
            public int CurrentLine { get; private set; }
            public int CurrentColumn { get; private set; }

            public int GetAbsoluteIndex() => this.StartingIndex + this.LocalIndex;

            public void Advance(int indexShift, int lineShift, int currentColumn)
            {
                this.LocalIndex += indexShift;
                this.CurrentLine += lineShift;
                this.CurrentColumn = currentColumn;

                _holder._version++;
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
            // todo checks
            _generations.Pop();
        }

        public int Depth => _generations.Count;
        public int Version => _version;

        public int GetCurrentLine() => this.GetLastGeneration()?.CurrentLine ?? 0;

        public int GetAbsoluteIndex() => this.GetLastGeneration()?.GetAbsoluteIndex() ?? 0;

        public int GetCurrentColumn() => this.GetLastGeneration()?.CurrentColumn ?? 0;

        public int GetStartingIndex() => this.GetLastGeneration()?.StartingIndex ?? 0;

        public int GetLocalIndex()
        {
            var lastGeneration = _generations.Peek();
            var localIndex = lastGeneration.LocalIndex;

            return localIndex;
        }

        public bool IsEnd()
        {
            // todo checks
            var lastGeneration = _generations.Peek();
            var absoluteIndex = lastGeneration.StartingIndex + lastGeneration.LocalIndex;
            if (absoluteIndex > this.Text.Length)
            {
                throw new NotImplementedException();
            }

            return absoluteIndex == this.Text.Length;
        }

        public void Advance(int indexShift, int lineShift, int currentColumn)
        {
            this.GetLastGeneration().Advance(indexShift, lineShift, currentColumn);
        }

        public char GetCurrentChar()
        {
            // todo checks
            var absoluteIndex = this.GetAbsoluteIndex();
            return this.Text[absoluteIndex];
        }

        public char GetLocalChar(int localIndex)
        {
            // todo checks
            var absoluteIndex = this.GetStartingIndex() + localIndex;
            return this.Text[absoluteIndex];
        }

        public void AdvanceByChar()
        {
            // todo checks
            this.Advance(1, 0, this.GetCurrentColumn() + 1);
        }

        public char? GetPreviousAbsoluteChar()
        {
            // todo: checks
            var absoluteIndex = this.GetAbsoluteIndex();
            if (absoluteIndex == 0)
            {
                return null;
            }

            return this.Text[absoluteIndex - 1];
        }

        public char? TryGetNextLocalChar()
        {
            if (this.IsEnd())
            {
                throw new NotImplementedException(); // todo
            }

            var wantedIndex = this.GetAbsoluteIndex() + 1;
            if (wantedIndex == this.Text.Length)
            {
                return null;
            }

            return this.Text[wantedIndex];
        }

        #endregion
    }
}
