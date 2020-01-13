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
            public Generation(
                int startIndex,
                int line,
                int column)
            {
                this.StartIndex = startIndex;
                this.IndexOffset = 0;
                this.Line = line;
                this.Column = column;
            }

            public int StartIndex { get; }
            public int IndexOffset { get; private set; }
            public int Line { get; private set; }
            public int Column { get; private set; }

            public void Advance(int indexShift, int lineShift, int column)
            {
                this.IndexOffset += indexShift;
                this.Line += lineShift;
                this.Column = column;
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
            var rootGeneration = new Generation(
                0,
                0,
                0);
            _generations.Push(rootGeneration);
            _version = 1;
        }

        #endregion

        #region ITextProcessingContext Members

        public string Text { get; }

        public void RequestGeneration()
        {
            int startIndex;
            int line;
            int column;

            if (_generations.Count == 0)
            {
                startIndex = 0;
                line = 0;
                column = 0;
            }
            else
            {
                var lastGeneration = _generations.Peek();

                startIndex =
                    lastGeneration.StartIndex +
                    lastGeneration.IndexOffset;
                line = lastGeneration.Line;
                column = lastGeneration.Column;
            }

            var generation = new Generation(
                startIndex,
                line,
                column);

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

        public int StartIndex => _generations.Peek().StartIndex;

        public int IndexOffset => _generations.Peek().IndexOffset;

        public int Line => _generations.Peek().Line;

        public int Column => _generations.Peek().Column;

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

            var desiredAbsoluteIndex = this.GetIndex() + indexShift;
            if (desiredAbsoluteIndex > this.Text.Length)
            {
                throw new IndexOutOfRangeException("Cannot advance beyond end of text.");
            }

            var generation = _generations.Peek();
            generation.Advance(indexShift, lineShift, currentColumn);
            _version++;
        }

        #endregion
    }
}
