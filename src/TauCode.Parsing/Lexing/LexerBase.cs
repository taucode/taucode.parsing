using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        #region Nested

        private struct CaretControlCharsSkipResult
        {
            public CaretControlCharsSkipResult(int charsSkipped, int linesSkipped)
            {
                if (charsSkipped <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(charsSkipped));
                }

                if (linesSkipped <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(linesSkipped));
                }

                this.CharsSkipped = charsSkipped;
                this.LinesSkipped = linesSkipped;
            }

            public int CharsSkipped { get; }
            public int LinesSkipped { get; }
        }

        #endregion

        #region Fields

        private string _input;
        private readonly List<ITokenExtractor> _tokenExtractors;
        private bool _tokenExtractorsInited;

        #endregion

        #region Constructor

        protected LexerBase()
        {
            _tokenExtractors = new List<ITokenExtractor>();
        }

        #endregion

        #region Abstract

        protected abstract void InitTokenExtractors();

        #endregion

        #region Private

        private CaretControlCharsSkipResult SkipCaretControlChars()
        {
            var totalLinesSkipped = 0;

            var startIndex = this.CurrentCharIndex;
            var index = startIndex;

            while (true)
            {
                if (this.IsEndAtIndex(index))
                {
                    break;
                }

                var c = this.GetCharAtIndex(index);

                if (c == LexingHelper.Cr)
                {
                    var nextChar = this.TryGetCharAtIndex(index + 1);
                    if (nextChar.HasValue)
                    {
                        if (nextChar.Value == LexingHelper.Lf)
                        {
                            // got CRLF
                            index += 2;
                            totalLinesSkipped++;
                        }
                        else
                        {
                            // got CR only
                            index += 1;
                            totalLinesSkipped++;
                        }
                    }
                    else
                    {
                        // got CR only, and there is no more chars.
                        index++;
                        totalLinesSkipped++;
                        break;
                    }
                }
                else if (c == LexingHelper.Lf)
                {
                    // got LF
                    index++;
                    totalLinesSkipped++;
                }
                else
                {
                    // no line-breaks; let's get out of this loop.
                    break;
                }
            }

            var totalDelta = index - startIndex;

            return new CaretControlCharsSkipResult(totalDelta, totalLinesSkipped);
        }

        #endregion

        #region Protected

        protected int CurrentCharIndex { get; private set; }

        protected int CurrentLine { get; private set; }

        protected int CurrentColumn { get; private set; }

        protected bool IsEndAtIndex(int index)
        {
            if (index > _input.Length)
            {
                // no one should ever query such a thing.
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentPosition());
            }

            if (index == _input.Length)
            {
                return true;
            }

            return false;
        }

        protected bool IsEnd() => this.IsEndAtIndex(this.CurrentCharIndex);

        protected Position GetCurrentPosition()
        {
            var line = this.CurrentLine;
            var column = this.CurrentColumn;

            return new Position(line, column);
        }

        protected char GetCharAtIndex(int index)
        {
            if (this.IsEndAtIndex(index))
            {
                // no one should ever query such a thing.
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentPosition());
            }

            return _input[index];
        }

        protected char GetCurrentChar() => this.GetCharAtIndex(this.CurrentCharIndex);

        protected char? TryGetCharAtIndex(int index)
        {
            if (this.IsEndAtIndex(index))
            {
                return null;
            }

            return _input[index];
        }

        protected void Advance(int shift = 1)
        {
            if (shift <= 0 || this.CurrentCharIndex + shift > _input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shift));
            }

            this.CurrentCharIndex += shift;
            this.CurrentColumn += shift;
        }

        protected List<ITokenExtractor> GetSuitableTokenExtractors(char firstChar)
        {
            return _tokenExtractors.Where(x => x.AllowsFirstChar(firstChar)).ToList();
        }

        protected void AddTokenExtractor(ITokenExtractor tokenExtractor)
        {
            if (tokenExtractor == null)
            {
                throw new ArgumentNullException(nameof(tokenExtractor));
            }

            _tokenExtractors.Add(tokenExtractor);
        }

        #endregion

        #region ILexer Members

        public IList<IToken> Lexize(string input)
        {
            if (!_tokenExtractorsInited)
            {
                this.InitTokenExtractors();
                _tokenExtractorsInited = true;
            }

            _input = input ?? throw new ArgumentNullException(nameof(input));
            this.CurrentCharIndex = 0;
            this.CurrentLine = 0;
            this.CurrentColumn = 0;

            var list = new List<IToken>();

            while (true)
            {
                if (this.IsEnd())
                {
                    return list;
                }

                var c = this.GetCurrentChar();

                if (LexingHelper.IsInlineWhiteSpace(c))
                {
                    this.Advance();
                    continue;
                }

                if (LexingHelper.IsCaretControl(c))
                {
                    var caretControlCharsSkipResult = this.SkipCaretControlChars();
                    this.Advance(caretControlCharsSkipResult.CharsSkipped);
                    this.CurrentLine += caretControlCharsSkipResult.LinesSkipped;
                    this.CurrentColumn = 0;

                    continue;
                }

                var tokenExtractors = this.GetSuitableTokenExtractors(c);
                IToken nextToken = null;

                foreach (var tokenExtractor in tokenExtractors)
                {
                    var result = tokenExtractor.Extract(_input, this.CurrentCharIndex, this.CurrentLine,
                        this.CurrentColumn);
                    nextToken = result.Token;

                    if (nextToken != null)
                    {
                        this.Advance(result.PositionShift);

                        this.CurrentLine += result.LineShift;
                        this.CurrentColumn = result.CurrentColumn.Value;

                        nextToken = result.Token;
                        break;
                    }
                }

                if (nextToken == null)
                {
                    throw new LexingException($"Unexpected char: '{c}'.",
                        this.GetCurrentPosition()); // todo: ut this, and all LexingException-s/CreateInternalErrorLexingException-s.
                }

                if (nextToken.HasPayload)
                {
                    list.Add(nextToken);
                }
            }
        }

        #endregion
    }
}
