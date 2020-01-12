namespace TauCode.Parsing.TextProcessing
{
    public interface ITextProcessor
    {
        bool AcceptsFirstChar(char c);

        TextProcessingResult Process(ITextProcessingContext context);

        bool IsBusy { get; }
    }
}
