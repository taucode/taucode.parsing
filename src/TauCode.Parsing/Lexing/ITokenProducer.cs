using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public interface ITokenProducer
    {
        TextProcessingContext Context { get; set; }

        IToken Produce();
    }
}
