using System;

namespace TauCode.Lab.Extensions.EmailValidation
{
    public static class EmailValidationExtensions
    {
        public const int MaxLocalPartLength = 64;
        public const int MaxEmailLength = 254;
        public const int MaxSubDomainLength = 63;

        internal const int MaxLocalPartSegmentCount = MaxEmailLength / 2;

        [ThreadStatic]
        private static EmailValidator EmailValidator;

        private static readonly EmailValidationSettings DefaultEmailValidationSettings = EmailValidationSettings.CreateDefault();

        public static EmailValidationResult ValidateEmail(string email, EmailValidationSettings settings = null)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (EmailValidator == null)
            {
                EmailValidator = new EmailValidator();
            }

            EmailValidator.Settings = settings ?? DefaultEmailValidationSettings;

            var span = email.AsSpan();

            return EmailValidator.Validate(span);
        }

        public static bool IsValidEmail(this string email, EmailValidationSettings settings = null) =>
            ValidateEmail(email, settings).IsSuccessful();
    }
}
