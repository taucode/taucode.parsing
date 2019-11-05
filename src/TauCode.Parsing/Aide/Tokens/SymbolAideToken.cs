namespace TauCode.Parsing.Aide.Tokens
{
    public class SymbolAideToken : AideToken
    {
        public SymbolAideToken(AideSymbolValue value, string name)
            : base(name)
        {
            this.Value = value;
        }

        public SymbolAideToken(char c, string name)
            : this(AideHelper.SymbolTokenFromChar(c), name)
        {
        }

        public AideSymbolValue Value { get; }
    }
}
