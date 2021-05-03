using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Lab.Extensions.EmailValidation
{
    public class EmailValidationSettings
    {
        #region Static

        public static IList<char> GetDefaultAllowedSymbols()
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
                AllowedSymbols = GetDefaultAllowedSymbols(),
            };
        }

        #endregion

        #region Fields

        internal HashSet<char> EffectiveAllowedSymbols;

        #endregion

        #region Public

        public bool AllowComments { get; set; }

        public IList<char> AllowedSymbols
        {
            get => EffectiveAllowedSymbols.ToList();
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var hashSet = value.ToHashSet();
                if (hashSet.Except(GetDefaultAllowedSymbols()).Any())
                {
                    throw new ArgumentException($"Invalid acceptable characters.", nameof(value)); // todo.
                }

                this.EffectiveAllowedSymbols = hashSet;
            }
        }

        #endregion
    }
}
