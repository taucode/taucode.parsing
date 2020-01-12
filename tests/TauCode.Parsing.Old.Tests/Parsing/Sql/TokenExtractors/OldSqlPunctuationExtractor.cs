using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Old.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Old.Tests.Parsing.Sql.TokenExtractors
{
    public class OldSqlPunctuationExtractor : OldTokenExtractorBase
    {
        public OldSqlPunctuationExtractor()
            : base(SqlPunctuationFirstCharPredicate)
        {
        }

        private static bool SqlPunctuationFirstCharPredicate(char c)
        {
            return c.IsIn('(', ')', ',');
        }

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();

            var position = new Position(this.StartingLine, this.StartColumn);
            var consumedLength = this.LocalCharIndex;

            return new PunctuationToken(str.Single(), position, consumedLength);
        }

        protected override OldCharChallengeResult ChallengeCurrentChar()
        {
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return OldCharChallengeResult.Continue;
            }

            return OldCharChallengeResult.Finish; // whatever it is - it's a single-char token extractor.
        }

        protected override OldCharChallengeResult ChallengeEnd() => OldCharChallengeResult.Finish;
    }
}
