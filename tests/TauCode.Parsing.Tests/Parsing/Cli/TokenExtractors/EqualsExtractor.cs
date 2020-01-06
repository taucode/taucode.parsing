using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    // todo clean up
    public class EqualsExtractor : TokenExtractorBase
    {
        public EqualsExtractor(/*ILexingEnvironment environment*/)
            : base(/*environment,*/ c => c == '=')
        {
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            throw new NotImplementedException();
            //return new PunctuationToken('=');
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var pos = this.GetLocalPosition();

            if (pos == 0)
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
