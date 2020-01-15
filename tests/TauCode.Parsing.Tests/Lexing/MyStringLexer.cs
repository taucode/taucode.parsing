using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardProducers;

namespace TauCode.Parsing.Tests.Lexing
{
    public class MyStringLexer : LexerBase
    {
        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
            {
                new WhiteSpaceProducer(),
                new CLangStringProducer(),
            };
        }
    }
}
