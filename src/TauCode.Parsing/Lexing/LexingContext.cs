using System.Collections.Generic;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public class LexingContext : TextProcessingContext, ILexingContext
    {
        #region Fields

        private readonly List<IToken> _tokens;

        #endregion

        #region Constructor

        public LexingContext(string text)
            : base(text)
        {
            _tokens = new List<IToken>();
        }

        #endregion

        #region Public

        public IList<IToken> GetTokenList() => _tokens;

        #endregion

        #region ILexingContext Members

        public IReadOnlyList<IToken> Tokens => _tokens;

        public IToken GetLastToken()
        {
            if (_tokens.Count == 0)
            {
                return null;
            }

            return _tokens[_tokens.Count - 1];
        }

        #endregion
    }
}
