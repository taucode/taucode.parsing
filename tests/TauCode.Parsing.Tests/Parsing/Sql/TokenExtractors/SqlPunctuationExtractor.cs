using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;

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

            throw new NotImplementedException();
            //return new PunctuationToken(str.Single());
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            throw new NotImplementedException();
            //var pos = this.GetLocalPosition();

            //if (pos == 0)
            //{
            //    return CharChallengeResult.Continue;
            //}

            //return CharChallengeResult.Finish; // whatever it is - it's a single-char token extractor.
        }

        protected override CharChallengeResult ChallengeEnd() => CharChallengeResult.Finish;
    }
}
