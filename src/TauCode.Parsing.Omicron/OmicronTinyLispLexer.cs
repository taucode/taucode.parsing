using TauCode.Parsing.Omicron.Producers;

namespace TauCode.Parsing.Omicron
{
    public class OmicronTinyLispLexer : OmicronLexerBase
    {
        protected override IOmicronTokenProducer[] CreateProducers()
        {
            return new IOmicronTokenProducer[]
            {
                new WhiteSpaceProducer(),
                new PunctuationProducer(),
                new StringProducer(),
                new IntegerProducer(),
                new SymbolProducer(),
                new KeywordProducer(),
                new CommentProducer(),
            };
        }
    }
}
