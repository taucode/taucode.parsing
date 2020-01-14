using System;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp;

namespace TauCode.Parsing.Omicron.Producers
{
    public class SymbolProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            if (this.Context.GetCurrentChar().IsAcceptableSymbolNameChar())
            {
                throw new NotImplementedException();
            }
            else
            {
                return null;
            }
        }
    }
}
