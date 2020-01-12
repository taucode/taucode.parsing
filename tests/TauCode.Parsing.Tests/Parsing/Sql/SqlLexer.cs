using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlLexer : LexerBase
    {
        protected override IList<ITokenExtractor> CreateTokenExtractors()
        {
            return new List<ITokenExtractor>
            {
                new WordExtractor(),
                new SqlPunctuationExtractor(),
                new IntegerExtractor(new[]
                {
                    typeof(PunctuationToken),
                }),
                new SqlIdentifierExtractor(),
            };
        }
    }
}
