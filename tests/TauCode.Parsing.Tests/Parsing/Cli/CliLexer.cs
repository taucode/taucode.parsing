using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliLexer : LexerBase
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new List<IGammaTokenExtractor>
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
