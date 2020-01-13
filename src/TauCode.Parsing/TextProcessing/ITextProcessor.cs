namespace TauCode.Parsing.TextProcessing
{
    public interface ITextProcessor
    {
        bool AcceptsFirstChar(char c); // todo: looks redundant.

        TextProcessingResult Process(ITextProcessingContext context);
    }
}
