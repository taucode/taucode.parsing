using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    // todo: clean up
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

        /// <summary>
        /// Index of current char, regardless line feeds, tabs etc.
        /// </summary>
        //private int _currentPosition;

        //private int _currentColumn;
        //private int _currentLine;

        private readonly List<ITokenExtractor> _tokenExtractors;

        private bool _tokenExtractorsInited;

        #endregion

        #region Constructor

        // todo clean up
        protected LexerBase( /*ILexingEnvironment environment = null*/)
        {
            //this.Environment = environment ?? StandardLexingEnvironment.Instance;
            _tokenExtractors = new List<ITokenExtractor>();
        }


        #endregion

        #region Abstract

        protected abstract void InitTokenExtractors();

        #endregion

        #region Protected

        protected int CurrentCharIndex { get; private set; }
        protected int CurrentLine { get; private set; }
        protected int CurrentColumn { get; private set; }

        //protected bool IsEnd() => _currentPosition == _input.Length;

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

        //protected bool IsEnd() => this.CurrentCharIndex == _input.Length;
        protected bool IsEnd() => this.IsEndAtIndex(this.CurrentCharIndex);

        protected Position GetCurrentPosition()
        {
            var line = this.CurrentLine;
            var column = this.CurrentColumn;

            return new Position(line, column);
        }

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateUnexpectedEndOfInputException(this.GetCurrentPosition());
            }

            //return _input[_currentPosition];
            return _input[this.CurrentCharIndex];
        }

        //protected int GetCurrentPosition() => _currentPosition;

        protected void Advance(int shift = 1)
        {
            if (shift <= 0 || /*_currentPosition*/ this.CurrentCharIndex + shift > _input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shift));
            }

            //_currentPosition += shift;
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

        //public ILexingEnvironment Environment { get; }

        public IList<IToken> Lexize(string input)
        {
            if (!_tokenExtractorsInited)
            {
                this.InitTokenExtractors();
                _tokenExtractorsInited = true;
            }

            _input = input ?? throw new ArgumentNullException(nameof(input));
            //_currentPosition = 0;
            //_currentLine = 0;
            //_currentColumn = 0;
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
                //var pos = this.GetCurrentPosition();

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

        //protected char? GetNextChar()
        //{
        //    if (this.IsEnd())
        //    {
        //        throw new NotImplementedException(); // todo error, why are we here?
        //    }

        //    var wantedIndex = this.CurrentCharIndex + 1;
        //    if (wantedIndex == _input.Length)
        //    {
        //        return null;
        //    }

        //    return _input[wantedIndex];
        //}

        protected char GetCharAtIndex(int index)
        {
            if (this.IsEndAtIndex(index))
            {
                throw new NotImplementedException(); // error todo
            }

            return _input[index];
        }

        protected char? TryGetCharAtIndex(int index)
        {
            if (this.IsEndAtIndex(index))
            {
                throw new NotImplementedException(); // todo error we should not query such a thing.
            }

            var wantedIndex = this.CurrentCharIndex + 1;
            if (wantedIndex == _input.Length)
            {
                return null;
            }

            return _input[wantedIndex];
        }

        private CaretControlCharsSkipResult SkipCaretControlChars()
        {
            //var totalCharsSkipped = 0;
            var totalLinesSkipped = 0;

            //int delta;

            var startIndex = this.CurrentCharIndex;
            var index = startIndex;

            while (true)
            {
                if (this.IsEndAtIndex(index))
                {
                    break;
                }

                //var c = this.GetCurrentChar();
                var c = this.GetCharAtIndex(index);

                if (c == '\r')
                {
                    var nextChar = this.TryGetCharAtIndex(index + 1);
                    if (nextChar.HasValue)
                    {
                        if (nextChar.Value == '\n')
                        {
                            // got CRLF
                            index += 2;
                            totalLinesSkipped++;

                            //throw new NotImplementedException();
                            //delta = 2; // \r, \n
                            //totalCharsSkipped += delta;
                            //this.Advance(delta);
                            //totalLinesSkipped++;
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
                        //this.Advance();
                        throw new NotImplementedException();
                    }
                }
                else if (c == '\n')
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
    }
}
