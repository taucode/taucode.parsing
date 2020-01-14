using System;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Omicron.Producers
{
    public class StringProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var c = this.Context.GetCurrentChar();
            if (c == '"')
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
