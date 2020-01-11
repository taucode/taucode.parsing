﻿using System;
using System.Collections.Generic;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lab.CommonLab;
using TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql
{
    public class SqlLexer : LexerBaseLab
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new List<IGammaTokenExtractor>
            {
                new WordExtractorLab(),
                new SqlPunctuationExtractor(),
                new IntegerExtractorLab(new List<Type>
                {
                    typeof(PunctuationToken),
                }),
                new SqlIdentifierExtractor(),
            };
        }
    }
}
