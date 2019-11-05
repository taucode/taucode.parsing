using System;
using TauCode.Parsing.Tests.Tokens;

namespace TauCode.Parsing.Tests
{
    public static class Helper
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
