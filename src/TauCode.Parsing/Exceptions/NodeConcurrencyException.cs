namespace TauCode.Parsing.Exceptions
{
    public class NodeConcurrencyException : ParseClauseFailedException
    {
        public NodeConcurrencyException(
            object[] partialParsingResults,
            IToken token,
            INode[] rivalNodes)
            : base(BuildMessage(token, rivalNodes), partialParsingResults)
        {
            this.Token = token;
            this.RivalNodes = rivalNodes;
        }

        public IToken Token { get; }

        public INode[] RivalNodes { get; }

        private static string BuildMessage(IToken token, INode[] rivalNodes)
        {
            throw new System.NotImplementedException();
        }
    }
}
