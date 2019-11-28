using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class SymbolToken : EnumToken<SymbolValue>
    {
        #region Constructors

        public SymbolToken(
            SymbolValue value,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(value, name, properties)
        {
        }

        public SymbolToken(
            char c,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : this(LexerHelper.SymbolTokenFromChar(c), name, properties)
        {
        }

        #endregion
    }
}
