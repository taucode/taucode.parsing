using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Producers
{
    public class SqlPunctuationProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var c = context.GetCurrentChar();

            if (c.IsIn('(', ')', ','))
            {
                var index = context.GetIndex();
                var position = context.GetCurrentPosition();
                context.AdvanceByChar();
                return new PunctuationToken(this.Context.Text[index], position, 1);
            }

            return null;
        }
    }
}
