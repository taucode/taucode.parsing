using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Omicron
{
    public interface IOmicronTokenProducer
    {
        TextProcessingContext Context { get; set; }

        IToken Produce();
    }
}
