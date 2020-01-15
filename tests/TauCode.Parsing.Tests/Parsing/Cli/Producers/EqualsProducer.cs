using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    public class EqualsProducer : ITokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var c = context.GetCurrentChar();
            if (c == '=')
            {
                var position = context.GetCurrentPosition();
                context.AdvanceByChar();
                return new PunctuationToken(c, position, 1);
            }

            return null;
        }
    }
}
