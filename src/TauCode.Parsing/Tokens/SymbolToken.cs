namespace TauCode.Parsing.Tokens
{
    public class SymbolToken : EnumToken<SymbolValue>
    {
        #region Constructors

        public SymbolToken(SymbolValue value)
            : base(value)
        {
        }

        public SymbolToken(SymbolValue value, string name)
            : base(value, name)
        {
        }

        public SymbolToken(char c)
            : this(LexerHelper.SymbolTokenFromChar(c))
        {
        }

        public SymbolToken(char c, string name)
            : this(LexerHelper.SymbolTokenFromChar(c), name)
        {
        }


        #endregion
    }
}
