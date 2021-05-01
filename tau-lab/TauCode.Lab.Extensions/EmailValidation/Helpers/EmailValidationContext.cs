using System;

namespace TauCode.Lab.Extensions.EmailValidation.Helpers
{
    public ref struct EmailValidationContext
    {
        public EmailValidationContext(ReadOnlySpan<char> input)
        {
            this.Index = 0;
            this.Span = input;
        }

        public int Index;
        public ReadOnlySpan<char> Span;
    }
}
