using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class CliLexer : LexerBaseLab
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new List<IGammaTokenExtractor>
            {
                new IntegerExtractorLab(new List<Type>()),
                new TermExtractor(),
                new KeyExtractor(),
                new StringExtractorLab(),
                new PathExtractor(),
                new EqualsExtractor(),
            };
        }
    }
}
