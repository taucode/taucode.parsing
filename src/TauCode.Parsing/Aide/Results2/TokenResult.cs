using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    public class TokenResult : IAideResult2
    {
        public TokenResult(IToken token)
        {
            // todo checks
            this.Token = token;
            this.Arguments = new List<string>();
        }

        public IToken Token { get; }
        public string Name => Token.Name;
        public IList<string> Arguments { get; }
    }
}
