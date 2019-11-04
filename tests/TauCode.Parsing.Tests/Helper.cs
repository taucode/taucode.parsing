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

                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}
