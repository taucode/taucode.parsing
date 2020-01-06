using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    // todo: clean up
    public abstract class LexerBase : ILexer
    {
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
        protected LexerBase(/*ILexingEnvironment environment = null*/)
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
        protected bool IsEnd() => this.CurrentCharIndex == _input.Length;

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateUnexpectedEndOfInputException();
            }

            //return _input[_currentPosition];
            return _input[this.CurrentCharIndex];
        }

        //protected int GetCurrentPosition() => _currentPosition;

        protected void Advance(int shift = 1)
        {
            if (shift < 0 || /*_currentPosition*/ this.CurrentCharIndex + shift > _input.Length)
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
                    throw new NotImplementedException();
                }

                var tokenExtractors = this.GetSuitableTokenExtractors(c);
                IToken nextToken = null;

                foreach (var tokenExtractor in tokenExtractors)
                {
                    var result = tokenExtractor.Extract(_input, this.CurrentCharIndex, this.CurrentLine, this.CurrentColumn);
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
                    throw new LexingException($"Unexpected char: '{c}'.");
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
