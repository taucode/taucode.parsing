using System.Collections.Generic;
using TauCode.Parsing.Exceptions;
using TauCode.Utils.Lab;

namespace TauCode.Parsing.Lexing
{
    public static class LexingHelper
    {
        internal static readonly HashSet<char> SpaceChars = new HashSet<char>(new[] { ' ', '\t', '\r', '\n' });
        internal static readonly HashSet<char> LineBreakChars = new HashSet<char>(new[] { '\r', '\n' });

        private static readonly HashSet<char> IntegerFirstChars;
        private static readonly HashSet<char> Digits;
        private static readonly HashSet<char> StandardPunctuationChars;
        private static readonly HashSet<char> LatinLetters;

        static LexingHelper()
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
                '#',
                '$',
                '%',
                '^',
                '&',
                '*',
                '|',
                '/',
                '+',
                '-',
                '[',
                ']',
                '(',
                ')',
                '{',
                '}',
                '\\',
                '.',
                ',',
            });
            StandardPunctuationChars = new HashSet<char>(punctList);

            var latinLetters = new List<char>();
            latinLetters.AddCharRange('a', 'z');
            latinLetters.AddCharRange('A', 'Z');

            LatinLetters = new HashSet<char>(latinLetters);
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

        public static bool IsSpace(char c) => SpaceChars.Contains(c);

        public static bool IsLineBreak(char c) => LineBreakChars.Contains(c);
        
        public static bool IsIntegerFirstChar(char c) => IntegerFirstChars.Contains(c);

        public static bool IsDigit(char c) => Digits.Contains(c);

        public static bool IsStandardPunctuationChar(char c) => StandardPunctuationChars.Contains(c); // todo: ut this; use c# interactive to get them all punctuations.

        public static bool IsLatinLetter(char c) => LatinLetters.Contains(c);
    }
}
