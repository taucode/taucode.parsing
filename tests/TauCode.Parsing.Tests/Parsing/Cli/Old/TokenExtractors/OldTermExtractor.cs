using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Cli.Old.TokenExtractors
{
    public class OldTermExtractor : TokenExtractorBase
    {
        public OldTermExtractor()
            : base(IsTermFirstChar)
        {
        }

        private static bool IsTermFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c);

        protected override void ResetState()
        {
            // idle
        }
        
        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;
            var token = new TextToken(
                TermTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);

            return token;
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (this.LocalCharIndex == 0)
            {
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (c == '-')
            {
                if (this.GetPreviousChar() == '-')
                {
                    // two '-' cannot go in a row within a <term>.
                    return CharChallengeResult.GiveUp;
                }

                return CharChallengeResult.Continue;
            }

            if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
            {
                return CharChallengeResult.Continue;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                var previousChar = this.GetPreviousChar();

                if (previousChar == '-')
                {
                    return CharChallengeResult.GiveUp; // term cannot end with '-'.
                }
                else
                {
                    return CharChallengeResult.Finish;
                }
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                if (this.GetPreviousChar() == '-')
                {
                    return CharChallengeResult.GiveUp; // term cannot end with '-'
                }
                else
                {
                    return CharChallengeResult.Finish;
                }
            }

            return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            if (this.GetPreviousChar() == '-')
            {
                return CharChallengeResult.GiveUp;
            }

            return CharChallengeResult.Finish;
        }
    }
}
