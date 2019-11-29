using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp
{
    public static class TinyLispHelper
    {
        internal static readonly char[] SpaceChars = { ' ', '\t', '\r', '\n' };
        internal static readonly char[] LineBreakChars = { '\r', '\n' };
        internal static char[] PunctuationChars = { '(', ')', '\'', '`', '.', ',' };

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



        internal static bool IsAcceptableSymbolNamePunctuationChar(this char c) => AcceptableSymbolNamePunctuationChars.Contains(c);

        internal static bool IsAcceptableSymbolNameChar(this char c) =>
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

                //var isValid =
                //    char.IsDigit(c) ||
                //    char.IsLetter(c) ||
                //    c.IsAcceptableSymbolNamePunctuationChar() ||
                //    c == '_';

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

        public static Punctuation CharToPunctuation(char c)
        {
            // todo: dictionary mapping <char> <--> <punctuation>
            switch (c)
            {
                case '(':
                    return Punctuation.LeftParenthesis;

                case ')':
                    return Punctuation.RightParenthesis;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
