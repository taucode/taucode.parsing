namespace TauCode.Parsing.TextProcessing
{
    public abstract class TextProcessorBase : ITextProcessor
    {
        #region ITextProcessor Members

        public abstract bool AcceptsFirstChar(char c);

        public abstract TextProcessingResult Process(ITextProcessingContext context);

        #endregion
    }
}
