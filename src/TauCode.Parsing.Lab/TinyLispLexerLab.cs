using System;
using System.Collections.Generic;
using TauCode.Parsing.Lab.CommonLab;
using TauCode.Parsing.Lab.TinyLispLab;

namespace TauCode.Parsing.Lab
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
            };
        }
    }
}
