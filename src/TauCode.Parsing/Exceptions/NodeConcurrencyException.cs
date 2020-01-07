using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class NodeConcurrencyException : ParseClauseFailedException
    {
        public NodeConcurrencyException(
            IToken token,
            INode[] concurrentNodes,
            object[] partialParsingResults)
            : base("More than one node accepted the token.", partialParsingResults)
        {
            this.Token = token;
            this.ConcurrentNodes = concurrentNodes;
        }

        public IToken Token { get; }

        public INode[] ConcurrentNodes { get; }
    }
}
