using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;

namespace TauCode.Parsing.Old.Tests.Parsing.Cli.TokenExtractors
{
    public class OldKeyExtractor : OldTokenExtractorBase
    {
        public OldKeyExtractor()
            : base(c => c == '-')
        {
        }

        protected override void ResetState()
        {
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            var position = new Position(this.StartLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;
            var token = new OldTextToken(OldKeyTextClass.Instance, OldNoneTextDecoration.Instance, str, position, consumedLength);
            return token;
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return OldCharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (index == 1)
            {
                if (c == '-')
                {
                    return OldCharChallengeResult.Continue;
                }

                if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
                {
                    return OldCharChallengeResult.Continue;
                }

                return OldCharChallengeResult.GiveUp;
            }

            if (index == 2 && c == '-')
            {
                if (this.GetPreviousChar() == '-')
                {
                    return OldCharChallengeResult.GiveUp; // 3 hyphens cannot be.
                }

                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsDigit(c) || LexingHelper.IsLatinLetter(c))
            {
                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) || c == '=')
            
            {
                return OldCharChallengeResult.Finish;
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
