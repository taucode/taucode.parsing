using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Old.TokenExtractors
{
    public class OldTermExtractor : OldTokenExtractorBase
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

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();

            if (this.LocalCharIndex == 0)
            {
                return OldCharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (c == '-')
            {
                if (this.GetPreviousChar() == '-')
                {
                    // two '-' cannot go in a row within a <term>.
                    return OldCharChallengeResult.GiveUp;
                }

                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
            {
                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                var previousChar = this.GetPreviousChar();

                if (previousChar == '-')
                {
                    return OldCharChallengeResult.GiveUp; // term cannot end with '-'.
                }
                else
                {
                    return OldCharChallengeResult.Finish;
                }
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                if (this.GetPreviousChar() == '-')
                {
                    return OldCharChallengeResult.GiveUp; // term cannot end with '-'
                }
                else
                {
                    return OldCharChallengeResult.Finish;
                }
            }

            return OldCharChallengeResult.GiveUp;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            if (this.GetPreviousChar() == '-')
            {
                return OldCharChallengeResult.GiveUp;
            }

            return OldCharChallengeResult.Finish;
        }
    }
}
