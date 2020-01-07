using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    public class SqlPunctuationExtractor : TokenExtractorBase
    {
        public SqlPunctuationExtractor()
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

            var position = new Position(this.StartingLine, this.StartingColumn);
            var consumedLength = this.LocalCharIndex;

            return new PunctuationToken(str.Single(), position, consumedLength);
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var index = this.LocalCharIndex;

            if (index == 0)
            {
                return CharChallengeResult.Continue;
            }

            return CharChallengeResult.Finish; // whatever it is - it's a single-char token extractor.
        }

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Finish;
    }
}
