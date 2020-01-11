using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Old.TextDecorations;
using TauCode.Parsing.Old.Tokens;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;

namespace TauCode.Parsing.Tests.Parsing.CliOld.TokenExtractors
{
    public class OldPathExtractor : OldTokenExtractorBase
    {
        public OldPathExtractor()
            : base(IsPathFirstChar)
        {
        }

        private static bool IsPathFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c) ||
            c.IsIn('\\', '/', '.', '!', '~', '$', '%', '-', '+');

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            var token = new OldTextToken(
                PathTextClass.Instance,
                OldNoneTextDecoration.Instance,
                str,
                position,
                consumedLength);

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

            if (IsPathFirstChar(c) || c == ':')
            {
                return OldCharChallengeResult.Continue;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return OldCharChallengeResult.Finish;
            }

            return OldCharChallengeResult.GiveUp;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            return OldCharChallengeResult.Finish;
        }
    }
}
