using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public interface ITokenProducer
    {
        ITextProcessingContext Context { get; set; }

        IToken Produce();
    }
}
