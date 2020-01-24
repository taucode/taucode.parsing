using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardProducers
{
    public class WhiteSpaceTokenProducer : WhiteSpaceProducer
    {
        protected override IToken ProduceImpl(Position position, int delta)
        {
            return new WhiteSpaceToken(position, delta);
        }
    }
}
