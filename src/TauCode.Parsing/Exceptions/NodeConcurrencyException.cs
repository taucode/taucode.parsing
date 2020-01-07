using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class NodeConcurrencyException : ParseClauseFailedException
    {
        public NodeConcurrencyException(
            IToken token,
            INode[] rivalNodes,
            object[] partialParsingResults)
            : base("More than one node accepted the token.", partialParsingResults)
        {
            this.Token = token;
            this.RivalNodes = rivalNodes;
        }

        public IToken Token { get; }

        public INode[] RivalNodes { get; }
    }
}
