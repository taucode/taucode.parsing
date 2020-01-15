using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispPunctuationProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;

            var c = text[context.Index];
            var punctuation = TinyLispHelper.TryCharToPunctuation(c);

            if (punctuation.HasValue)
            {
                var position = new Position(Context.Line, Context.Column);
                this.Context.AdvanceByChar();
                return new LispPunctuationToken(
                    punctuation.Value,
                    position,
                    1);
            }

            return null;
        }
    }
}
