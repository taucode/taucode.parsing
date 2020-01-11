using TauCode.Parsing.Lexing;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Old.TokenExtractors
{
    public class OldEqualsExtractor : OldTokenExtractorBase
    {
        public OldEqualsExtractor()
            : base(c => c == '=')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new PunctuationToken('=', position, consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                // 0th is always accepted.
                return OldCharChallengeResult.Continue;
            }

            return OldCharChallengeResult.Finish;
        }

        protected override OldCharChallengeResult ChallengeEnd()
        {
            return OldCharChallengeResult.Finish; // exactly one char is guaranteed to be consumed; no problem with end of input.
        }
    }
}
