using System;
using System.Collections.Generic;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp
{
    // todo clean up
    public static class TinyLispHelper
    {
        internal static readonly HashSet<char> SpaceChars = new HashSet<char>(new[] { ' ', '\t', '\r', '\n' });
        internal static readonly HashSet<char> LineBreakChars = new HashSet<char>(new[] { '\r', '\n' });
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

        public static bool IsSpace(char c) => SpaceChars.Contains(c);

        public static bool IsLineBreak(char c) => LineBreakChars.Contains(c);

        public static bool IsPunctuation(char c) => PunctuationChars.Contains(c);

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

        public static char PunctuationToChar(this Punctuation punctuation)
        {
            // todo: dictionary mapping <char> <--> <punctuation>
            switch (punctuation)
            {
                case Punctuation.LeftParenthesis:
                    return '(';

                case Punctuation.RightParenthesis:
                    return ')';

                case Punctuation.Quote:
                    return '\'';

                case Punctuation.BackQuote:
                    return '`';

                case Punctuation.Period:
                    return '.';

                case Punctuation.Comma:
                    return ',';

                default:
                    throw new ArgumentOutOfRangeException(nameof(punctuation), punctuation, null);
            }
        }
    }
}
