using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    // todo: paring of NVARCHAR(100) will produce Precision = 100, Scale = null, Size = null, while it should produce Precision = null, Scale = null, Size = 100
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
            //var c = this.GetCurrentChar();
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
