namespace TauCode.Parsing.Tokens
{
    public class SymbolToken : IToken
    {
        public SymbolToken(SymbolValue value)
        {
            this.Value = value;
        }

        public SymbolToken(char c)
            : this(LexerHelper.SymbolTokenFromChar(c))
        {

        }

        public SymbolValue Value { get; }
    }
}
