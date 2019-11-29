using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexer2
{
    // todo clean up
    public abstract class LexerBase : ILexer
    {
        private string _input;
        private int _pos;

        //private readonly HashSet<char> _spaceChars;
        //private readonly HashSet<char> _lineBreakChars;
        private readonly Func<char, bool> _spacePredicate;
        private readonly Func<char, bool> _lineBreakPredicate;

        private readonly List<ITokenExtractor> _tokenExtractors;
        private bool _tokenExtractorsInited;

        protected LexerBase(
            Func<char, bool> spacePredicate,
            Func<char, bool> lineBreakPredicate)
        {
            // todo check args
            // todo: line breaks must be contained in space chars.
            //_spaceChars = new HashSet<char>(spaceChars);
            //_lineBreakChars = new HashSet<char>(lineBreakChars);

            _spacePredicate = spacePredicate;
            _lineBreakPredicate = lineBreakPredicate;

            _tokenExtractors = new List<ITokenExtractor>();
        }

        protected bool IsEnd() => _pos == _input.Length;

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexerHelper.CreateUnexpectedEndOfInputException();
            }

            return _input[_pos];
        }

        protected int GetCurrentPosition() => _pos;

        protected bool IsSpaceChar(char c) => _spacePredicate(c);
        //{
        //    return _spaceChars.Contains(c);
        //}

        protected void Advance(int shift = 1)
        {
            // todo checks
            _pos += shift;
        }

        protected List<ITokenExtractor> GetSuitableTokenExtractors(char firstChar)
        {
            return _tokenExtractors.Where(x => x.AllowsFirstChar(firstChar)).ToList();
        }

        protected abstract void InitTokenExtractors();

        protected void AddTokenExtractor(ITokenExtractor tokenExtractor)
        {
            // todo checks
            _tokenExtractors.Add(tokenExtractor);
        }

        public List<IToken> Lexize(string input)
        {
            if (!_tokenExtractorsInited)
            {
                this.InitTokenExtractors();
                _tokenExtractorsInited = true;
            }

            _input = input ?? throw new ArgumentNullException(nameof(input));
            _pos = 0;
            var list = new List<IToken>();

            while (true)
            {
                if (this.IsEnd())
                {
                    return list;
                }

                var c = this.GetCurrentChar();
                var pos = this.GetCurrentPosition();

                if (this.IsSpaceChar(c))
                {
                    this.Advance();
                    continue;
                }

                var tokenExtractors = this.GetSuitableTokenExtractors(c);
                IToken nextToken = null;

                foreach (var tokenExtractor in tokenExtractors)
                {
                    var result = tokenExtractor.Extract(_input, pos);
                    nextToken = result.Token;

                    if (nextToken != null)
                    {
                        this.Advance(result.Shift);
                        nextToken = result.Token;
                        break;
                    }

                    //if (result.Token == null)
                    //{
                    //    if (tokenExtractors.Count == 1)
                    //    {
                    //        // no one else will try to 
                    //        throw result.Exception;
                    //    }
                    //    else
                    //    {
                    //        continue;
                    //    }
                    //}
                    //else
                    //{
                    //    // got token
                    //    throw new NotImplementedException();
                    //}
                }

                if (nextToken == null)
                {
                    throw new NotImplementedException();
                }

                if (nextToken is NullToken)
                {
                    // ignore it
                    //list.Add(nextToken); // todo: remove.
                }
                else
                {
                    list.Add(nextToken);
                }

                //throw new NotImplementedException(); // error - none produced the result
            }
        }
    }
}
