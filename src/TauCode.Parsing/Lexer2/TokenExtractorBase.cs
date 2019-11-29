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

        private readonly List<ITokenExtractor> _successors;

        protected TokenExtractorBase(char[] spaceChars, char[] allowedFirstChars)
        {
            // todo checks

            this.SpaceChars = new HashSet<char>(spaceChars);
            _allowedFirstChars = new HashSet<char>(allowedFirstChars);
            _successors = new List<ITokenExtractor>();
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

        protected virtual bool AllowsEndAfterProduction() => true;

        protected virtual bool AllowsSpaceAfterProduction() => true;

        protected virtual bool AllowsCharAfterProduction(char c)
        {
            foreach (var successor in _successors)
            {
                if (successor.AllowsFirstChar(c))
                {
                    return true;
                }
            }

            return false;
        }

        protected string ExtractResultString()
        {
            var str = _input.Substring(_startPos, _localPos);
            return str;
        }

        protected char GetPreviousChar()
        {
            // todo range checks
            return _input[this.GetAbsolutePosition() - 1];
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
                        var token = this.ProduceResult();
                        //this.Advance();
                        
                        if (token == null)
                        {
                            // yes, maybe. e.g. \BeginFoo instead of \BeginBlockDefinition
                            throw new NotImplementedException();
                        }

                        // ok, we've got result and advanced, but if everything is ok with next?
                        if (this.IsEnd())
                        {
                            var check = this.AllowsEndAfterProduction();
                            if (!check)
                            {
                                throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            var c = this.GetCurrentChar();

                            if (this.IsSpaceChar(c))
                            {
                                var check = this.AllowsSpaceAfterProduction();
                                if (!check)
                                {
                                    throw new NotImplementedException();
                                }
                            }
                            else
                            {
                                var check = this.AllowsCharAfterProduction(c);
                                if (!check)
                                {
                                    throw new NotImplementedException();
                                }
                            }
                        }

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
            if (cond)
            {
                return TestCharResult.Continue;
            }
            else
            {
                return TestCharResult.NotAllowed;
            }
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

        public void AddSuccessors(params TokenExtractorBase[] successors)
        {
            _successors.AddRange(successors);
        }
    }
}
