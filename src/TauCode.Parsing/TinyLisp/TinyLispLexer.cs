using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TinyLisp.TokenExtractors;

namespace TauCode.Parsing.TinyLisp
{
    public class TinyLispLexer : LexerBase
    {
        protected override IList<IGammaTokenExtractor> CreateTokenExtractors()
        {
            return new IGammaTokenExtractor[]
            {
                new TinyLispCommentExtractor(),
                new TinyLispSymbolExtractor(),
                new TinyLispPunctuationExtractor(),
                new IntegerExtractor(new Type[]
                {
                    typeof(TinyLispPunctuationExtractor),
                }),
                new TinyLispKeywordExtractor(),
                new TinyLispStringExtractor(), 
            };
        }
    }
}
