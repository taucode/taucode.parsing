using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Omicron.Producers
{
    public class PunctuationProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var c = this.Context.GetCurrentChar();
            if (TinyLispHelper.IsPunctuation(c)) // todo: can optimize; two checks will be performed
            {
                this.Context.AdvanceByChar();
                return new PunctuationToken(c, this.Context.GetCurrentPosition(), 1);
            }
            else
            {
                return null;
            }
        }
    }
}
