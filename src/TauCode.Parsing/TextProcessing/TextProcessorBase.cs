using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.TextProcessing
{
    public abstract class TextProcessorBase : ITextProcessor
    {
        #region Fields

        private bool _isBusy;

        #endregion

        #region ITextProcessor Members

        public abstract bool AcceptsFirstChar(char c);

        public abstract TextProcessingResult Process(ITextProcessingContext context);

        public bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                if (value == _isBusy)
                {
                    throw new TextProcessingException(
                        $"Suspicious operation: setting '{nameof(IsBusy)}' to the same value it has now ({value}).");
                }

                _isBusy = value;
            }
        }

        #endregion

        public virtual ITextProcessingContext AlphaGetContext() => null;

    }
}
