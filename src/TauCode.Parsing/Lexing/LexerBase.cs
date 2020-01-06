using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public abstract class LexerBase : ILexer
    {
        #region Fields

        private string _input;

        /// <summary>
        /// Index of current char, regardless line feeds, tabs etc.
        /// </summary>
        private int _currentPosition;

        private int _currentColumn;
        private int _currentLine;

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

        protected bool IsEnd() => _currentPosition == _input.Length;

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateUnexpectedEndOfInputException();
            }

            return _input[_currentPosition];
        }

        protected int GetCurrentPosition() => _currentPosition;

        protected void Advance(int shift = 1)
        {
            if (shift < 0 || _currentPosition + shift > _input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(shift));
            }

            _currentPosition += shift;
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
            _currentPosition = 0;
            _currentLine = 0;
            _currentColumn = 0;

            var list = new List<IToken>();

            while (true)
            {
                if (this.IsEnd())
                {
                    return list;
                }

                var c = this.GetCurrentChar();
                var pos = this.GetCurrentPosition();

                throw new NotImplementedException();

                //if (this.Environment.IsSpace(c))
                //{
                //    this.Advance();
                //    continue;
                //}

                //var tokenExtractors = this.GetSuitableTokenExtractors(c);
                //IToken nextToken = null;

                //foreach (var tokenExtractor in tokenExtractors)
                //{
                //    var result = tokenExtractor.Extract(_input, pos);
                //    nextToken = result.Token;

                //    if (nextToken != null)
                //    {
                //        this.Advance(result.Shift);
                //        nextToken = result.Token;
                //        break;
                //    }
                //}

                //if (nextToken == null)
                //{
                //    throw new LexingException($"Unexpected char: '{c}'.");
                //}

                //if (nextToken.HasPayload)
                //{
                //    list.Add(nextToken);
                //}
            }
        }

        #endregion
    }
}
