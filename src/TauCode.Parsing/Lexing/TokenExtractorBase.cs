using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public abstract class TokenExtractorBase : ITokenExtractor
    {
        #region Fields

        private string _input;
        private int _startPos;
        private int _localPos;
        private readonly List<ITokenExtractor> _successors;

        #endregion

        #region Constructor

        protected TokenExtractorBase(
            ILexingEnvironment environment,
            Func<char, bool> firstCharPredicate)
        {

            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            FirstCharPredicate = firstCharPredicate ?? throw new ArgumentNullException(nameof(firstCharPredicate));

            _successors = new List<ITokenExtractor>();
        }

        #endregion

        #region Abstract

        protected abstract void ResetState();

        protected abstract IToken ProduceResult();

        protected abstract CharChallengeResult ChallengeCurrentChar();

        protected abstract CharChallengeResult ChallengeEnd();

        #endregion

        #region Protected

        protected readonly ILexingEnvironment Environment;

        protected Func<char, bool> FirstCharPredicate { get; }

        protected bool IsEnd() => this.GetAbsolutePosition() == _input.Length;

        protected int GetLocalPosition() => _localPos;

        protected int GetAbsolutePosition() => _startPos + _localPos;

        protected char GetLocalChar(int localPosition)
        {
            return _input[_startPos + localPosition];
        }

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

        protected void Advance()
        {
            _localPos++;
        }

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateInternalErrorException();
            }

            var absPos = this.GetAbsolutePosition();
            var c = _input[absPos];
            return c;
        }

        public void AddSuccessors(params TokenExtractorBase[] successors) => _successors.AddRange(successors);

        #endregion

        #region ITokenExtractor Members

        public TokenExtractionResult Extract(string input, int position)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));

            if (position < 0 || position >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            _startPos = position;
            _localPos = 0;

            this.ResetState();

            while (true)
            {
                if (this.IsEnd())
                {
                    var challengeEnd = this.ChallengeEnd();

                    switch (challengeEnd)
                    {
                        case CharChallengeResult.Finish:
                            var token = this.ProduceResult();
                            if (token == null)
                            {
                                // possible situation. e.g. in LISP '+1488' looks like as a symbol at the beginning, but at the end would appear
                                // an integer, and symbol extractor would refuse deliver such a result as a symbol.
                                return new TokenExtractionResult(0, null);
                            }
                            else
                            {
                                return new TokenExtractionResult(this.GetLocalPosition(), token);
                            }

                        case CharChallengeResult.GiveUp:
                            return new TokenExtractionResult(0, null);

                        case CharChallengeResult.Error:
                            throw new LexingException("Unexpected end of input.");

                        default:
                            throw LexingHelper.CreateInternalErrorException();
                    }
                }

                var testCharResult = this.ChallengeCurrentChar();

                switch (testCharResult)
                {
                    case CharChallengeResult.GiveUp:
                        return new TokenExtractionResult(0, null); // this extractor failed to recognize the whole token, no problem.

                    case CharChallengeResult.Continue:
                        this.Advance();
                        break;

                    case CharChallengeResult.Finish:
                        var token = this.ProduceResult();

                        if (token == null)
                        {
                            return new TokenExtractionResult(0, null);
                        }

                        // check if next char is ok.
                        if (!this.IsEnd())
                        {
                            var upcomingChar = this.GetCurrentChar();
                            if (!Environment.IsSpace(upcomingChar))
                            {
                                var check = this.AllowsCharAfterProduction(upcomingChar);
                                if (!check)
                                {
                                    throw new LexingException($"Unexpected char: '{upcomingChar}'.");
                                }
                            }
                        }

                        return new TokenExtractionResult(this.GetLocalPosition(), token);

                    default:
                        throw new LexingException($"Internal error. Unexpected test char result: '{testCharResult}'.");
                }
            }
        }

        public virtual bool AllowsFirstChar(char c) => FirstCharPredicate(c);

        #endregion
    }
}
