using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Tokens;
using TauCode.Utils.Lab;

namespace TauCode.Parsing.Lexizing
{
    public static class LexerHelper
    {
        private static readonly HashSet<char> IntegerFirstChars;
        private static readonly HashSet<char> Digits;
        private static readonly HashSet<char> StandardPunctuationChars;

        static LexerHelper()
        {
            var list = new List<char>
            {
                '+',
                '-',
            };

            list.AddCharRange('0', '9');
            IntegerFirstChars = new HashSet<char>(list);

            var digits = new List<char>();
            digits.AddCharRange('0', '9');
            Digits = new HashSet<char>(digits);

            var punctList = new List<char>();
            punctList.AddRange(new []
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
            StandardPunctuationChars = new HashSet<char>(punctList);
        }

        #region Exceptions

        internal static LexerException CreateUnexpectedEndOfInputException()
        {
            return new LexerException("Unexpected end of input.");
        }

        internal static LexerException CreateUnexpectedCharException(char c)
        {
            return new LexerException($"Unexpected char: '{c}'.");
        }

        internal static LexerException CreateEmptyTokenException()
        {
            return new LexerException("Empty token.");
        }

        #endregion

        public static SymbolValue SymbolTokenFromChar(char c)
        {
            switch (c)
            {
                case '(':
                    return SymbolValue.LeftParenthesis;

                case ')':
                    return SymbolValue.RightParenthesis;

                case ',':
                    return SymbolValue.Comma;

                case '=':
                    return SymbolValue.Equals;

                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }

        public static bool IsIntegerFirstChar(char c) => IntegerFirstChars.Contains(c);

        public static bool IsDigit(char c) => Digits.Contains(c);

        public static bool IsStandardPunctuationChar(char c)
        {
            throw new NotImplementedException();
        }
    }
}
