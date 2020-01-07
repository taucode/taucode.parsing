﻿using System.Collections.Generic;
using TauCode.Extensions;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Lexing
{
    public static class LexingHelper
    {
        private static readonly HashSet<char> IntegerFirstChars;
        private static readonly HashSet<char> Digits;
        private static readonly HashSet<char> StandardPunctuationChars;
        private static readonly HashSet<char> LatinLetters;

        public const char Cr = '\r';
        public const char Lf = '\n';

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
                '=',
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
                '"',
                '\'',
                ':',
                ';',
                '`',
            });
            StandardPunctuationChars = new HashSet<char>(punctList);

            var latinLetters = new List<char>();
            latinLetters.AddCharRange('a', 'z');
            latinLetters.AddCharRange('A', 'Z');

            LatinLetters = new HashSet<char>(latinLetters);
        }

        public static bool IsIntegerFirstChar(char c) => IntegerFirstChars.Contains(c);

        public static bool IsDigit(char c) => Digits.Contains(c);

        public static bool IsStandardPunctuationChar(char c) => StandardPunctuationChars.Contains(c);

        public static bool IsLatinLetter(char c) => LatinLetters.Contains(c);

        internal static LexingException CreateInternalErrorLexingException(Position position)
        {
            return new LexingException("Internal error.", position);
        }

        public static bool IsInlineWhiteSpace(char c) => c.IsIn(' ', '\t');

        public static bool IsCaretControl(char c) => c.IsIn('\r', '\n');

        public static bool IsInlineWhiteSpaceOrCaretControl(char c) => IsInlineWhiteSpace(c) || IsCaretControl(c);
    }
}
