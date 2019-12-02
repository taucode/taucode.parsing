using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexizing.StandardTokenExtractors
{
    public class IntegerExtractor : TokenExtractorBase
    {
        public IntegerExtractor(
            Func<char, bool> spacePredicate,
            Func<char, bool> lineBreakPredicate)
            : base(
                spacePredicate,
                lineBreakPredicate,
                LexizingHelper.IsIntegerFirstChar)
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

            return new IntegerToken(str);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                if (this.FirstCharPredicate(c))
                {
                    return CharChallengeResult.Continue;
                }
                else
                {
                    throw new LexerException("Internal error."); // how on earth we could even get here?
                }
            }

            if (LexizingHelper.IsDigit(c))
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
                return CharChallengeResult.Finish;
            }

            if (this.SpacePredicate(c) || char.IsWhiteSpace(c))
            {
                return CharChallengeResult.Finish;
            }

            // other chars like letters and stuff => not allowed.
            return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            var localPos = this.GetLocalPosition();

            if (localPos == 0)
            {
                throw new LexerException("Internal error."); // todo copy/paste
            }
            else if (localPos == 1)
            {
                var c = this.GetLocalChar(0);
                if (LexizingHelper.IsDigit(c))
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
