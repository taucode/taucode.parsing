using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public abstract class TokenExtractorBase : ITokenExtractor
    {
        #region Fields

        private string _input;
        private readonly List<ITokenExtractor> _successors;

        #endregion

        #region Constructor

        protected TokenExtractorBase(Func<char, bool> firstCharPredicate)
        {

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

        protected int StartingAbsoluteCharIndex { get; private set; }

        protected int StartingLine { get; private set; }

        protected int LineShift { get; private set; }

        protected int StartingColumn { get; private set; }

        protected int CurrentColumn { get; private set; }

        protected int LocalCharIndex { get; private set; }

        protected Func<char, bool> FirstCharPredicate { get; }

        protected bool IsEnd() => this.StartingAbsoluteCharIndex + this.LocalCharIndex == _input.Length;

        protected int GetAbsoluteCharIndex() => this.StartingAbsoluteCharIndex + this.LocalCharIndex;

        protected char GetLocalChar(int localIndex)
        {
            if (localIndex < 0)
            {
                throw new NotImplementedException(); // todo: you shouldn't. error.
            }

            return _input[this.StartingAbsoluteCharIndex + localIndex];
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
            //var str = _input.Substring(_startPos, _localPos);
            var str = _input.Substring(this.StartingAbsoluteCharIndex, this.LocalCharIndex);
            return str;
        }

        protected void Advance()
        {
            this.LocalCharIndex++;
            this.CurrentColumn++;

            //_localPos++;
        }

        protected void SkipSingleLineBreak()
        {
            var c = this.GetCurrentChar();
            int indexShift;
            
            if (c == LexingHelper.Cr)
            {
                var nextChar = this.GetNextChar();
                if (nextChar.HasValue)
                {
                    if (nextChar.Value == LexingHelper.Lf)
                    {
                        indexShift = 2; // got CRLF
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (c == LexingHelper.Lf)
            {
                throw new NotImplementedException();
            }
            else
            {
                // how on earth did we get here?
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
            }

            this.LocalCharIndex += indexShift;
            this.LineShift++;
            this.CurrentColumn = 0;
        }

        protected char GetCurrentChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
            }

            var absPos = this.GetAbsoluteCharIndex();
            var c = _input[absPos];
            return c;
        }

        protected char? GetNextChar()
        {
            if (this.IsEnd())
            {
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
            }

            var absIndex = this.StartingAbsoluteCharIndex + this.LocalCharIndex + 1;
            if (absIndex == _input.Length)
            {
                return null;
            }

            var c = _input[absIndex];
            return c;
        }

        protected char GetPreviousChar()
        {
            if (this.LocalCharIndex <= 0)
            {
                throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
            }

            return this.GetLocalChar(this.LocalCharIndex - 1);
        }

        protected Position GetStartingAbsolutePosition()
        {
            var line = this.StartingLine;
            var column = this.StartingColumn;
            return new Position(line, column);
        }

        protected Position GetCurrentAbsolutePosition()
        {
            var line = this.StartingLine + this.LineShift;
            var column = this.CurrentColumn;

            return new Position(line, column);
        }

        #endregion

        #region Public

        public void AddSuccessors(params TokenExtractorBase[] successors) => _successors.AddRange(successors);

        #endregion

        #region ITokenExtractor Members

        public TokenExtractionResult Extract(string input, int charIndex, int line, int column)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));

            if (charIndex < 0 || charIndex >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(charIndex));
            }

            this.StartingAbsoluteCharIndex = charIndex;

            this.StartingLine = line;
            this.LineShift = 0;

            this.StartingColumn = column;
            this.CurrentColumn = column;

            this.LocalCharIndex = 0;

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
                                return new TokenExtractionResult(null, 0, 0, null);
                            }
                            else
                            {
                                return new TokenExtractionResult(token, this.LocalCharIndex, this.LineShift, this.CurrentColumn);
                            }

                        case CharChallengeResult.GiveUp:
                            throw new NotImplementedException();

                        default:
                            // should never happen. this is an enum, after all.
                            throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
                    }
                }

                var testCharResult = this.ChallengeCurrentChar();

                switch (testCharResult)
                {
                    case CharChallengeResult.GiveUp:
                        // this extractor failed to recognize the whole token, no problem.
                        return new TokenExtractionResult(null, 0, 0, null);

                    case CharChallengeResult.Continue:
                        this.Advance(); // todo: deal with line breaks?
                        break;

                    case CharChallengeResult.Finish:
                        var token = this.ProduceResult();

                        if (token == null)
                        {
                            return new TokenExtractionResult(null, 0, 0, null);
                        }

                        // check if next char is ok.
                        if (!this.IsEnd())
                        {
                            var upcomingChar = this.GetCurrentChar();
                            if (!LexingHelper.IsInlineWhiteSpaceOrCaretControl(upcomingChar))
                            {
                                var check = this.AllowsCharAfterProduction(upcomingChar);
                                if (!check)
                                {
                                    throw new LexingException($"Unexpected char: '{upcomingChar}'.", this.GetCurrentAbsolutePosition());
                                }
                            }
                        }

                        return new TokenExtractionResult(token, this.LocalCharIndex, this.LineShift, this.CurrentColumn);

                    default:
                        // should never happen. this is enum.
                        throw new LexingException($"Internal error. Unexpected test char result: '{testCharResult}'.", this.GetCurrentAbsolutePosition());
                }
            }
        }

        public virtual bool AllowsFirstChar(char c) => FirstCharPredicate(c);

        #endregion
    }
}
