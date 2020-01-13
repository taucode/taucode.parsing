using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Old.Lexing
{
    public abstract class OldTokenExtractorBase : IOldTokenExtractor
    {
        #region Fields

        private string _input;
        private readonly List<IOldTokenExtractor> _successors;

        #endregion

        #region Constructor

        protected OldTokenExtractorBase(Func<char, bool> firstCharPredicate)
        {

            FirstCharPredicate = firstCharPredicate ?? throw new ArgumentNullException(nameof(firstCharPredicate));
            _successors = new List<IOldTokenExtractor>();
        }

        #endregion

        #region Abstract

        protected abstract void ResetState();

        protected abstract IToken ProduceResult();

        protected abstract OldCharChallengeResult ChallengeCurrentChar();

        protected abstract OldCharChallengeResult ChallengeEnd();

        #endregion

        #region Protected

        protected int StartAbsoluteCharIndex { get; private set; }

        protected int StartLine { get; private set; }

        protected int LineShift { get; private set; }

        protected int StartColumn { get; private set; }

        protected int CurrentColumn { get; private set; }

        protected int LocalCharIndex { get; private set; }

        protected Func<char, bool> FirstCharPredicate { get; }

        protected bool IsEnd() => this.StartAbsoluteCharIndex + this.LocalCharIndex == _input.Length;

        protected int GetAbsoluteCharIndex() => this.StartAbsoluteCharIndex + this.LocalCharIndex;

        protected char GetLocalChar(int localIndex)
        {
            if (localIndex < 0)
            {
                throw LexingHelper.CreateErrorInLogicLexingException();
            }

            return _input[this.StartAbsoluteCharIndex + localIndex];
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
            var str = _input.Substring(this.StartAbsoluteCharIndex, this.LocalCharIndex);
            return str;
        }

        protected void Advance()
        {
            this.LocalCharIndex++;
            this.CurrentColumn++;
        }

        protected void SkipSingleLineBreak()
        {
            var c = this.GetCurrentChar();
            int indexShift;
            
            if (c == LexingHelper.CR)
            {
                var nextChar = this.GetNextChar();
                if (nextChar.HasValue)
                {
                    if (nextChar.Value == LexingHelper.LF)
                    {
                        indexShift = 2; // got CRLF
                    }
                    else
                    {
                        indexShift = 1;
                    }
                }
                else
                {
                    // no more chars.
                    indexShift = 1;
                }
            }
            else if (c == LexingHelper.LF)
            {
                indexShift = 1;
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

            var absIndex = this.StartAbsoluteCharIndex + this.LocalCharIndex + 1;
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

        protected Position GetStartAbsolutePosition()
        {
            var line = this.StartLine;
            var column = this.StartColumn;
            return new Position(line, column);
        }

        protected Position GetCurrentAbsolutePosition()
        {
            var line = this.StartLine + this.LineShift;
            var column = this.CurrentColumn;

            return new Position(line, column);
        }

        #endregion

        #region Public

        public void AddSuccessors(params OldTokenExtractorBase[] successors) => _successors.AddRange(successors);

        #endregion

        #region ITokenExtractor Members

        public OldTokenExtractionResult Extract(string input, int charIndex, int line, int column)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));

            if (charIndex < 0 || charIndex >= input.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(charIndex));
            }

            this.StartAbsoluteCharIndex = charIndex;

            this.StartLine = line;
            this.LineShift = 0;

            this.StartColumn = column;
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
                        case OldCharChallengeResult.Finish:
                            var token = this.ProduceResult();
                            if (token == null)
                            {
                                // possible situation. e.g. in LISP '+1488' looks like as a symbol at the beginning, but at the end would appear
                                // an integer, and symbol extractor would refuse deliver such a result as a symbol.
                                return new OldTokenExtractionResult(null, 0, 0, null);
                            }
                            else
                            {
                                return new OldTokenExtractionResult(token, this.LocalCharIndex, this.LineShift, this.CurrentColumn);
                            }

                        case OldCharChallengeResult.GiveUp:
                            return new OldTokenExtractionResult(null, 0, 0, null);

                        default:
                            // should never happen. check your token extractor, it has error(s).
                            throw LexingHelper.CreateInternalErrorLexingException(this.GetCurrentAbsolutePosition());
                    }
                }

                var testCharResult = this.ChallengeCurrentChar();

                switch (testCharResult)
                {
                    case OldCharChallengeResult.GiveUp:
                        // this extractor failed to recognize the whole token, no problem.
                        return new OldTokenExtractionResult(null, 0, 0, null);

                    case OldCharChallengeResult.Continue:
                        this.Advance();
                        break;

                    case OldCharChallengeResult.Finish:
                        var token = this.ProduceResult();

                        if (token == null)
                        {
                            return new OldTokenExtractionResult(null, 0, 0, null);
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

                        return new OldTokenExtractionResult(token, this.LocalCharIndex, this.LineShift, this.CurrentColumn);

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
