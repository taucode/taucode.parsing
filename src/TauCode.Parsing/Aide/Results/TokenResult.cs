using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class TokenResult : IAideResult
    {
        #region Constructor

        public TokenResult(IToken token)
        {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
            this.Arguments = new List<string>();
        }

        #endregion

        #region Public

        public IToken Token { get; }

        #endregion

        #region IAideResult Members

        public string Name => Token.Name;

        public IList<string> Arguments { get; }

        #endregion
    }
}
