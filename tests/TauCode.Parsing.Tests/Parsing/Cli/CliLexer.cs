using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliLexer : LexerBase
    {
        protected override IList<ITokenExtractor> CreateTokenExtractors()
        {
            return new List<ITokenExtractor>
            {
                new IntegerExtractor(new List<Type>()),
                new TermExtractor(),
                new KeyExtractor(),
                new StringExtractor(),
                new PathExtractor(),
                new EqualsExtractor(),
            };
        }
    }
}
