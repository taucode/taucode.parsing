﻿using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlLexer : LexerBase
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new List<IGammaTokenExtractor>
            {
                new WordExtractor(),
                new SqlPunctuationExtractor(),
                new IntegerExtractor(new List<Type>
                {
                    typeof(PunctuationToken),
                }),
                new SqlIdentifierExtractor(),
            };
        }
    }
}
