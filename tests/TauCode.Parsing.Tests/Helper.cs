using System;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests
{
    public static class Helper
    {
        public static SymbolTokenValue SymbolTokenFromChar(char c)
        {
            switch (c)
            {
                case '(':
                    return SymbolTokenValue.LeftParenthesis;

                case ')':
                    return SymbolTokenValue.RightParenthesis;

                case ',':
                    return SymbolTokenValue.Comma;

                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}
