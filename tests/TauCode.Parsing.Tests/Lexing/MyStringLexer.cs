using System.Collections.Generic;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardEscapeProcessors;
using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TextDecorations;

namespace TauCode.Parsing.Tests.Lexing
{
    public class MyStringLexer : LexerBase
    {
        protected override IList<ITokenExtractor> CreateTokenExtractors()
        {
            return new List<ITokenExtractor>
            {
                new StringExtractorBase(
                    '"',
                    '"',
                    false,
                    DoubleQuoteTextDecoration.Instance,
                    new EscapeProcessorBase[]
                    {
                        new CLangSingleCharEscapeProcessor(),
                        new CLangU4EscapeProcessor(),
                    },
                    null)
            };
        }
    }
}
