using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexizing
{
    public static class LexerHelper
    {
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

        public static readonly char[] StandardSpaceChars = new[] { ' ', '\r', '\n', '\t' };
    }
}
