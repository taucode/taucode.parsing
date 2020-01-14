using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

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
                var position = this.Context.GetCurrentPosition();
                this.Context.AdvanceByChar();
                return new LispPunctuationToken(
                    TinyLispHelper.CharToPunctuation(c),
                    position,
                    1);
            }
            else
            {
                return null;
            }
        }
    }
}
