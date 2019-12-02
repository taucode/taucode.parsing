using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexizing
{
    // todo clean up
    public abstract class TokenExtractorBase : ITokenExtractor
    {
        protected Func<char, bool> SpacePredicate { get; }
        protected Func<char, bool> LineBreakPredicate { get; }
        protected Func<char, bool> FirstCharPredicate { get; }

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
            SpacePredicate = spacePredicate;
            LineBreakPredicate = lineBreakPredicate;
            FirstCharPredicate = firstCharPredicate;

            _successors = new List<ITokenExtractor>();
        }

        protected bool IsEnd() => this.GetAbsolutePosition() == _input.Length;

        protected int GetLocalPosition() => _localPos;

        protected int GetAbsolutePosition() => _startPos + _localPos;

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

        public TokenExtractionResult Extract(string input, int position)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));

            if (position < 0 || position >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            _startPos = position;
            _localPos = 0;

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
                            throw new LexerException("Unexpected end of input.");

                        default:
                            throw new LexerException("Internal error."); // todo copy/paste
                    }
                }

                var c = this.GetCurrentChar();
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
                            throw new LexerException($"Internal error. Token extractor of type '{this.GetType().FullName}' produced a null token.");
                        }

                        // check if next char is ok.
                        if (!this.IsEnd())
                        {
                            var upcomingChar = this.GetCurrentChar();
                            if (!this.SpacePredicate(upcomingChar))
                            {
                                var check = this.AllowsCharAfterProduction(upcomingChar);
                                if (!check)
                                {
                                    throw new LexerException($"Unexpected token: '{upcomingChar}'.");
                                }
                            }
                        }

                        return new TokenExtractionResult(this.GetLocalPosition(), token);

                    default:
                        throw new LexerException($"Internal error. Unexpected test char result: '{testCharResult}'.");
                }
            }
        }

        protected abstract IToken ProduceResult();

        protected void Advance()
        {
            _localPos++;
        }

        protected abstract CharChallengeResult ChallengeCurrentChar();

        protected abstract CharChallengeResult ChallengeEnd();

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw new LexerException("Internal error: trying to get current char at the end of input.");
            }

            var absPos = this.GetAbsolutePosition();
            var c = _input[absPos];
            return c;
        }

        public virtual bool AllowsFirstChar(char c) => FirstCharPredicate(c);

        public void AddSuccessors(params TokenExtractorBase[] successors) => _successors.AddRange(successors);
    }
}
