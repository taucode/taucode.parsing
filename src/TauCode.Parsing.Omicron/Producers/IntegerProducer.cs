using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Omicron.Producers
{
    public class IntegerProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            if (LexingHelper.IsIntegerFirstChar(this.Context.GetCurrentChar()))
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
