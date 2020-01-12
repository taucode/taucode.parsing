using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Old.Lexing.StandardTokenExtractors
{
    public class OldIntegerExtractor : OldTokenExtractorBase
    {
        public OldIntegerExtractor()
            : base(LexingHelper.IsIntegerFirstChar)
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

            var position = new Position(this.StartingLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;

            return new IntegerToken(str, position, consumedLength);
        }

        protected override void ResetState()
        {
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return OldCharChallengeResult.Continue; // MUST be ok.
            }

            if (LexingHelper.IsDigit(c))
            {
                return OldCharChallengeResult.Continue; // digits are always welcome in an integer.
            }

            if (c == '.' || c == '_')
            {
                // period ('.') is rarely a thing that you can delimit an integer within any grammar.
                // underscore ('_') is a punctuation mark, but nowadays it is usually a part of identifiers.

                return OldCharChallengeResult.GiveUp;
            }

            // other punctuation marks like Comma, (, ), others might delimit though...
            if (char.IsPunctuation(c))
            {
                var gotOnlySign = this.GotOnlySign();
                if (gotOnlySign)
                {
                    return OldCharChallengeResult.GiveUp;
                }

                return OldCharChallengeResult.Finish;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return OldCharChallengeResult.Finish;
            }

            // other chars like letters and stuff => not allowed.
            return OldCharChallengeResult.GiveUp;
        }

        private bool GotOnlySign()
        {
            if (this.LocalCharIndex == 1)
            {
                var firstChar = this.GetLocalChar(0);
                return firstChar.IsIn('+', '-');
            }

            return false;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            var localPos = this.LocalCharIndex;

            if (localPos == 0)
            {
                throw LexingHelper.CreateInternalErrorLexingException(
                    this.GetCurrentAbsolutePosition()); // how could we get to end if we are actually at the start (localPos == 0)?!
            }
            else if (localPos == 1)
            {
                var c = this.GetLocalChar(0);
                if (LexingHelper.IsDigit(c))
                {
                    return OldCharChallengeResult.Finish; // int consisting of a single digit - no problem.
                }
                else
                {
                    return OldCharChallengeResult.GiveUp; // not an int. let some another extractor deal with it.
                }
            }
            else
            {
                // we consumed more than one char, so it is guaranteed we've got a good int already
                return OldCharChallengeResult.Finish;
            }
        }
    }
}
