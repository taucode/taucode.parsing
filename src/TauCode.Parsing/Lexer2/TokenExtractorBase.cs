using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Lexer2
{
    public abstract class TokenExtractorBase : ITokenExtractor
    {
        private readonly HashSet<char> _allowedFirstChars;
        private string _input;
        private int _startPos;
        private int _localPos;

        protected TokenExtractorBase(char[] spaceChars, char[] allowedFirstChars)
        {
            // todo checks

            this.SpaceChars = new HashSet<char>(spaceChars);
            _allowedFirstChars = new HashSet<char>(allowedFirstChars);
        }

        protected HashSet<char> SpaceChars { get; }

        protected bool IsSpaceChar(char c)
        {
            return this.SpaceChars.Contains(c);
        }

        protected bool IsEnd()
        {
            return this.GetAbsolutePosition() == _input.Length;
        }

        protected int GetLocalPosition()
        {
            return _localPos;
        }

        protected abstract void Reset();

        protected int GetAbsolutePosition()
        {
            return _startPos + _localPos;
        }

        public TokenExtractionResult Extract(string input, int position)
        {
            // todo checks

            _input = input;
            _startPos = position;
            _localPos = 0;

            this.Reset();

            while (true)
            {
                //var absolutePos = position + localPos;

                //var c = input[absolutePos];

                if (this.IsEnd())
                {
                    throw new NotImplementedException();
                }

                var testCharResult = this.TestCurrentChar();

                switch (testCharResult)
                {
                    case TestCharResult.NotAllowed:
                        throw new NotImplementedException();

                    case TestCharResult.Continue:
                        this.Advance();
                        break;

                    case TestCharResult.End:
                        var token = this.ProduceResult(); // todo: must not be null
                        this.Advance();
                        return new TokenExtractionResult(this.GetLocalPosition(), token);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            throw new NotImplementedException();
        }

        protected abstract IToken ProduceResult();

        protected void Advance()
        {
            _localPos++;
        }

        protected TestCharResult ContinueIf(bool cond)
        {
            return cond ? TestCharResult.Continue : TestCharResult.NotAllowed;
        }

        protected abstract TestCharResult TestCurrentChar();
        //{
        //    var c = this.GetCurrentChar();
        //    var localPos = this.GetLocalPosition();

        //    if (localPos == 0)
        //    {
        //        return this.AllowsFirstChar(c) ? TestCharResult.Continue : TestCharResult.NotAllowed;
        //    }

        //    return this.TestCurrentCharImpl();

        //    //throw new NotImplementedException();
        //    //if (localPosition == 0)
        //    //{
        //    //    return this.AllowsFirstChar(c) ? TestCharResult.Continue : TestCharResult.NotAllowed;
        //    //}

        //    //return this.TestCharImpl(c, localPosition);
        //}

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw new NotImplementedException();
            }

            var absPos = this.GetAbsolutePosition();
            var c = _input[absPos];
            return c;
        }

        //protected abstract TestCharResult TestCurrentCharImpl();

        //protected abstract bool AllowsCharAtLocalPosition(char c, int localPos);

        public bool AllowsFirstChar(char firstChar)
        {
            return _allowedFirstChars.Contains(firstChar);
        }
    }
}
