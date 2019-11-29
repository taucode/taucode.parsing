using System;
using System.Collections.Generic;

namespace TauCode.Parsing.TinyLisp
{
    public static class TinyLispHelper
    {
        internal static readonly char[] SpaceChars = { ' ', '\t', '\r', '\n' };
        internal static readonly char[] LineBreakChars = { '\r', '\n' };

        private static readonly HashSet<char> AcceptableSymbolNameSymbols = new HashSet<char>(new[]
        {
            '~',
            '?',
            '!',
            '@',
            '$',
            '%',
            '^',
            '&',
            '*',
            '/',
            '+',
            '-',
            '[',
            ']',
            '{',
            '}',
            '\\',
        });

        internal static bool IsAcceptableSymbolNameSymbol(this char c) => AcceptableSymbolNameSymbols.Contains(c);


        public static bool IsValidSymbolName(string name, bool mustBeKeyword)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length == 0)
            {
                return false;
            }

            var start = 0;
            var actualNameLength = name.Length;

            if (mustBeKeyword)
            {
                if (name[0] != ':')
                {
                    return false;
                }

                start = 1;
                actualNameLength = name.Length - 1;
            }

            if (actualNameLength == 0)
            {
                return false;
            }

            var onlyDigits = true;

            for (var i = start; i < name.Length; i++)
            {
                var c = name[i];

                if (onlyDigits)
                {
                    if (char.IsDigit(c))
                    {
                        // still only digits, leave
                    }
                    else
                    {
                        onlyDigits = false;
                    }
                }

                var isValid =
                    char.IsDigit(c) ||
                    char.IsLetter(c) ||
                    c.IsAcceptableSymbolNameSymbol() ||
                    c == '_';

                if (!isValid)
                {
                    return false;
                }
            }

            if (onlyDigits)
            {
                return false;
            }

            return true;
        }
    }
}
