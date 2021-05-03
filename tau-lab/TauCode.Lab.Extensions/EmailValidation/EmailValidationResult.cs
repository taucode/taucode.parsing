namespace TauCode.Lab.Extensions.EmailValidation
{
    public readonly struct EmailValidationResult
    {
        public EmailValidationResult(EmailValidationError error, byte? errorPosition)
        {
            this.Error = error;
            this.ErrorPosition = errorPosition;

            if (!this.IsSuccessful())
            {
            }
        }

        public EmailValidationError Error { get; }
        public byte? ErrorPosition { get; }

        public bool IsSuccessful() => Error == EmailValidationError.NoError;
    }
}
