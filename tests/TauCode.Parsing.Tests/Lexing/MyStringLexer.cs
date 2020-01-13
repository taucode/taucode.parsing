using System.Collections.Generic;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Lexing
{
    public class MyStringLexer : LexerBase
    {
        protected override IList<ITokenExtractor> CreateTokenExtractors()
        {
            return new List<ITokenExtractor>
            {
                new MyStringExtractor(),
            };
        }
    }
}
