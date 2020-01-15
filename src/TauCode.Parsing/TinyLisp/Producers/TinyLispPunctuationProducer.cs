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
            if (TinyLispHelper.IsPunctuation(c)) // todo: can optimize; two checks will be performed
            {
                var position = new Position(Context.Line, Context.Column);
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
