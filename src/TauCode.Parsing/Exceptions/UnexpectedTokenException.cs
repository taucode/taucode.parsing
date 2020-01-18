using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class UnexpectedTokenException : ParseClauseFailedException
    {
        public UnexpectedTokenException(IToken token, object[] partialParsingResults)
            : base(BuildMessage(token), partialParsingResults)
        {
            this.Token = token;
        }

        private static string BuildMessage(IToken token)
        {
            return $"Unexpected token: {token}.";
        }

        public IToken Token { get; }
    }
}
