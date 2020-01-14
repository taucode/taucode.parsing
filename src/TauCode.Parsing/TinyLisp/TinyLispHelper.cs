using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Extensions;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp
{
    public static class TinyLispHelper
    {
        internal static readonly HashSet<char> PunctuationChars = new HashSet<char>(new char[] { '(', ')', '\'', '`', '.', ',' });

        private static readonly HashSet<char> AcceptableSymbolNamePunctuationChars = new HashSet<char>(new[]
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

        private static readonly IDictionary<char, Punctuation> PunctuationsByChar;
        private static readonly IDictionary<Punctuation, char> CharsByPunctuation;

        static TinyLispHelper()
        {
            var pairs = new[]
            {
                "( LeftParenthesis",
                ") RightParenthesis",
                "' Quote",
                "` BackQuote",
                ". Period",
                ", Comma",
            };

            PunctuationsByChar = pairs
                .Select(x => x.Split(' '))
                .ToDictionary(x => x[0].Single(), x => x[1].ToEnum<Punctuation>());

            CharsByPunctuation = PunctuationsByChar
                .ToDictionary(x => x.Value, x => x.Key);
        }

        internal static bool IsAcceptableSymbolNamePunctuationChar(this char c) => AcceptableSymbolNamePunctuationChars.Contains(c);

        // todo: shouldn't be extension.
        public static bool IsAcceptableSymbolNameChar(this char c) =>
            char.IsDigit(c) ||
            char.IsLetter(c) ||
            c == '_' ||
            AcceptableSymbolNamePunctuationChars.Contains(c);

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

                var isValid = IsAcceptableSymbolNameChar(c);

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

        public static bool IsPunctuation(char c) => PunctuationChars.Contains(c);

        public static Punctuation CharToPunctuation(char c)
        {
            var punctuation = PunctuationsByChar.GetOrDefault(c);
            if (punctuation == default)
            {
                throw new ArgumentOutOfRangeException(nameof(c), $"'{c}' is not a known punctuation character.");
            }

            return punctuation;
        }

        public static char PunctuationToChar(this Punctuation punctuation)
        {

            var c = CharsByPunctuation.GetOrDefault(punctuation);
            if (c == default)
            {
                throw new ArgumentOutOfRangeException(nameof(c), $"'{punctuation}' is not a known punctuation.");
            }

            return c;
        }
    }
}
