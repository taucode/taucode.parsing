using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class EqualsExtractor : TokenExtractorBase
    {
        public EqualsExtractor()
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

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                // 0th is always accepted.
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            throw new NotImplementedException();
        }
    }
}
