using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Lab.Extensions.EmailValidation
{
    public class EmailValidationSettings
    {
        #region Static

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


        #endregion

        #region Fields

        internal HashSet<char> EffectiveAllowedSpecialCharacters;

        #endregion

        #region Public

        public bool AllowComments { get; set; }

        public IList<char> AllowedSpecialCharacters
        {
            get => EffectiveAllowedSpecialCharacters.ToList();
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var hashSet = value.ToHashSet();
                if (hashSet.Except(GetDefaultAllowedSpecialCharacters()).Any())
                {
                    throw new ArgumentException($"Invalid acceptable characters.", nameof(value)); // todo.
                }

                this.EffectiveAllowedSpecialCharacters = hashSet;
            }
        }


        #endregion
    }
}
