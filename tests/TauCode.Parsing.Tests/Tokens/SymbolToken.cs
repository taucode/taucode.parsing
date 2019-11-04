using System;

namespace TauCode.Parsing.Tests.Tokens
{
    public class SymbolToken : IToken
    {
        public SymbolToken(SymbolTokenValue value)
        {
            this.Value = value;
        }

        public SymbolTokenValue Value { get; }

        public static SymbolToken FromChar(char c)
        {
            switch (c)
            {
                case '(':
                    return new SymbolToken(SymbolTokenValue.LeftParenthesis);

                case ')':
                    return new SymbolToken(SymbolTokenValue.RightParenthesis);

                default:
                    throw new ArgumentOutOfRangeException(nameof(c));
            }
        }
    }
}
