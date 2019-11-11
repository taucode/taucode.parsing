using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing
{
    public static class LexerHelper
    {
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

                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}
