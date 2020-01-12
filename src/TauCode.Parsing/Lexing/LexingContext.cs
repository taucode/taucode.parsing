using System.Collections.Generic;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp;

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

        #endregion
    }
}
