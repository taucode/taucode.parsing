using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TinyLisp.TokenExtractors;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexerLab : LexerBaseLab
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new IGammaTokenExtractor[]
            {
                new TinyLispCommentExtractorLab(),
                new TinyLispSymbolExtractorLab(),
                new TinyLispPunctuationExtractorLab(),
                new IntegerExtractorLab(new Type[]
                {
                    typeof(TinyLispPunctuationExtractorLab),
                }),
                new TinyLispKeywordExtractor(),
                new TinyLispStringExtractor(), 
            };
        }
    }
}
