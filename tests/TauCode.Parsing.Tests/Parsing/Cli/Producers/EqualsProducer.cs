using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    public class EqualsProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;

            var c = text[context.Index];
            if (c == '=')
            {
                var position = new Position(context.Line, context.Column);
                context.AdvanceByChar();
                return new PunctuationToken(c, position, 1);
            }

            return null;
        }
    }
}
