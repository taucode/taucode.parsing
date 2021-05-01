using System.Collections.Generic;

namespace TauCode.Lab.Extensions.EmailValidation
{
    public class EmailValidationSettings
    {
        public static IList<char> GetDefaultAllowedSpecialCharacters()
        {
            return new[]
            {
                '-',
                '+',
                '=',
                '!',
                '?',
                '%',
                '~',
                '$',
                '&',
                '/',
                '|',
                '{',
                '}',
                '#',
                '*',
                '\'',
            };
        }

        public static EmailValidationSettings CreateDefault()
        {
            return new EmailValidationSettings
            {
                AllowComments = true,
                AllowedSpecialCharacters = GetDefaultAllowedSpecialCharacters(),
            };
        }

        public bool AllowComments { get; set; }
        public IList<char> AllowedSpecialCharacters { get; set; }
    }
}
