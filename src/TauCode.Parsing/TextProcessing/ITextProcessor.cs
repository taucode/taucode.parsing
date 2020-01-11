namespace TauCode.Parsing.TextProcessing
{
    public interface ITextProcessor<out TProduct>
    {
        bool AcceptsFirstChar(char c);

        bool IsProcessing { get; }

        TextProcessingResult Process(ITextProcessingContext context);

        TProduct Produce(string text, int absoluteIndex, int consumedLength, Position position);
    }
}
