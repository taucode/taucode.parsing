using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardEscapeProcessors;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TinyLisp.Tokens;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.TokenExtractors
{
    public class TinyLispStringExtractor : StringExtractorBase
    {
        public TinyLispStringExtractor()
            : base(
                '"',
                '"',
                true,
                DoubleQuoteTextDecoration.Instance,
                new EscapeProcessorBase[]
                {
                    new CrEscapeProcessor(),
                    new LfEscapeProcessor(),
                },
                new Type[]
                {
                    typeof(PunctuationToken),
                    typeof(LispSymbolToken),
                    typeof(KeywordToken)
                })
        {
        }
    }
}
