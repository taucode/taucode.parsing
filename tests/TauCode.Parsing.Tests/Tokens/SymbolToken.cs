namespace TauCode.Parsing.Tests.Tokens
{
    public class SymbolToken : IToken
    {
        public SymbolToken(SymbolValue value)
        {
            this.Value = value;
        }

        public SymbolToken(char c)
            : this(Helper.SymbolTokenFromChar(c))
        {

        }

        public SymbolValue Value { get; }
    }
}
