namespace TauCode.Parsing.Tests.Tokens
{
    public class SymbolToken : IToken
    {
        public SymbolToken(SymbolTokenValue value)
        {
            this.Value = value;
        }

        public SymbolToken(char c)
            : this(Helper.SymbolTokenFromChar(c))
        {

        }

        public SymbolTokenValue Value { get; }
    }
}
