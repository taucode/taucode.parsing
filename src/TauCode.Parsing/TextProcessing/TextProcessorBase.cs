using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.TextProcessing
{
    public abstract class TextProcessorBase<TProduct> : ITextProcessor<TProduct>
    {
        private bool _isProcessing;

        public abstract bool AcceptsFirstChar(char c);

        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                if (value == _isProcessing)
                {
                    throw new TextProcessingException(
                        $"Suspicious operation: setting '{nameof(IsProcessing)}' to the same value it has now ({value}).");
                }

                _isProcessing = value;
            }
        }


        public abstract TextProcessingResult Process(ITextProcessingContext context);

        public abstract TProduct Produce(string text, int absoluteIndex, Position position, int consumedLength);
    }
}
