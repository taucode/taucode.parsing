using System;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Omicron.Producers
{
    public class KeywordProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            if (this.Context.GetCurrentChar() == ':')
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
