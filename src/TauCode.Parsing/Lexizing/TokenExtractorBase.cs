using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexizing
{
    public abstract class TokenExtractorBase : ITokenExtractor
    {
        private readonly Func<char, bool> _spacePredicate;
        private readonly Func<char, bool> _lineBreakPredicate;
        private readonly Func<char, bool> _firstCharPredicate;

        private string _input;
        private int _startPos;
        private int _localPos;

        private readonly List<ITokenExtractor> _successors;

        protected TokenExtractorBase(
            Func<char, bool> spacePredicate,
            Func<char, bool> lineBreakPredicate,
            Func<char, bool> firstCharPredicate)
        {
            // todo checks
            _spacePredicate = spacePredicate;
            _lineBreakPredicate = lineBreakPredicate;
            _firstCharPredicate = firstCharPredicate;

            _successors = new List<ITokenExtractor>();
        }

        protected bool IsSpaceChar(char c) => _spacePredicate(c);

        protected bool IsLineBreakChar(char c) => _lineBreakPredicate(c);

        protected bool IsEnd() => this.GetAbsolutePosition() == _input.Length;

        protected int GetLocalPosition() => _localPos;

        protected abstract void Reset();

        protected int GetAbsolutePosition() => _startPos + _localPos;

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
                if (this.IsEnd())
                {
                    var testEndResult = this.TestEnd();
                    if (testEndResult)
                    {
                        var token = this.ProduceResult();
                        if (token == null)
                        {
                            throw new NotImplementedException();
                        }
                        else
                        {
                            return new TokenExtractionResult(this.GetLocalPosition(), token);
                        }
                    }
                    else
                    {
                        throw new LexerException("Unexpected end of input.");
                    }
                }

                var testCharResult = this.TestCurrentChar();

                switch (testCharResult)
                {
                    case TestCharResult.NotAllowed:
                        throw new NotImplementedException();

                    case TestCharResult.Continue:
                        this.Advance();
                        break;

                    case TestCharResult.Finish:
                        var token = this.ProduceResult();

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
                        throw new ArgumentOutOfRangeException(); // todo
                }
            }
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

        protected abstract bool TestEnd();

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

        public bool AllowsFirstChar(char c) => _firstCharPredicate(c);

        public void AddSuccessors(params TokenExtractorBase[] successors) => _successors.AddRange(successors);
    }
}
