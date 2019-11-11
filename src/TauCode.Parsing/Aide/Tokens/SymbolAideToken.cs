using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Tokens
{
    public class SymbolAideToken : AideToken
    {
        public SymbolAideToken(SymbolValue value, string name)
            : base(name)
        {
            this.Value = value;
        }

        public SymbolAideToken(char c, string name)
            : this(LexerHelper.SymbolTokenFromChar(c), name)
        {
        }

        public SymbolValue Value { get; }
    }
}
