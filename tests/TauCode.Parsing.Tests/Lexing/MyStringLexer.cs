using TauCode.Parsing.Omicron;
using TauCode.Parsing.Omicron.Producers;

namespace TauCode.Parsing.Tests.Lexing
{
    public class MyStringLexer : OmicronLexerBase
    {
        protected override IOmicronTokenProducer[] CreateProducers()
        {
            return new IOmicronTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new OmicronCLangStringProducer(),
            };
        }
    }
}
