using System;
using TauCode.Extensions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardTokenExtractors
{
    public class IntegerExtractor : TokenExtractorBase
    {
        public IntegerExtractor(/*ILexingEnvironment environment*/)
            : base(
                //environment,
                LexingHelper.IsIntegerFirstChar)
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            if (str[0] == '+')
            {
                // omit leading '+'
                str = str.Substring(1);
            }

            // todo: remove this check
            if (!int.TryParse(str, out var dummy))
            {
                throw new NotImplementedException();
            }

            return new IntegerToken(str, Position.TodoErrorPosition, LexingHelper.TodoErrorConsumedLength);
        }

        protected override void ResetState()
        {
            // todo: idle now, but actually should contain things like heading sign '+', '-', etc...
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // MUST be ok.
            }

            if (LexingHelper.IsDigit(c))
            {
                return CharChallengeResult.Continue; // digits are always welcome in an integer.
            }

            if (c == '.' || c == '_')
            {
                // period ('.') is rarely a thing that you can delimit an integer within any grammar.
                // underscore ('_') is a punctuation mark, but nowadays it is usually a part of identifiers.

                return CharChallengeResult.GiveUp;
            }

            // other punctuation marks like Comma, (, ), others might delimit though...
            if (char.IsPunctuation(c))
            {
                var gotOnlySign = this.GotOnlySign();
                if (gotOnlySign)
                {
                    return CharChallengeResult.GiveUp;
                }

                return CharChallengeResult.Finish;
            }

            throw new NotImplementedException();

            //if (this.Environment.IsSpace(c) || char.IsWhiteSpace(c))
            //{
            //    return CharChallengeResult.Finish;
            //}

            //// other chars like letters and stuff => not allowed.
            //return CharChallengeResult.GiveUp;
        }

        private bool GotOnlySign()
        {
            var localPosition = this.GetLocalPosition();
            if (localPosition == 1)
            {
                var firstChar = this.GetLocalChar(0);
                return firstChar.IsIn('+', '-');
            }

            return false;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            var localPos = this.GetLocalPosition();

            if (localPos == 0)
            {
                throw LexingHelper.CreateInternalErrorException();
            }
            else if (localPos == 1)
            {
                var c = this.GetLocalChar(0);
                if (LexingHelper.IsDigit(c))
                {
                    return CharChallengeResult.Finish; // int consisting of a single digit - no problem.
                }
                else
                {
                    return CharChallengeResult.GiveUp; // not an int. let some another extractor deal with it.
                }
            }
            else
            {
                // we consumed more than one char, so it is guaranteed we've got a good int already
                return CharChallengeResult.Finish;
            }
        }
    }
}
