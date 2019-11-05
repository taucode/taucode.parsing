using System;

namespace TauCode.Parsing.Aide
{
    public static class AideHelper
    {
        public static AideSymbolValue SymbolTokenFromChar(char c)
        {
            switch (c)
            {
                case '(':
                    return AideSymbolValue.LeftParenthesis;

                case ')':
                    return AideSymbolValue.RightParenthesis;

                case ',':
                    return AideSymbolValue.Comma;

                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}
