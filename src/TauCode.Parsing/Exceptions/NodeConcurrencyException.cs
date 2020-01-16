using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class NodeConcurrencyException : ParseClauseFailedException
    {
        public NodeConcurrencyException(
            IToken token,
            IEnumerable<INode> concurrentNodes,
            object[] partialParsingResults)
            : base("More than one node accepted the token.", partialParsingResults)
        {
            // todo checks
            this.Token = token;
            this.ConcurrentNodes = concurrentNodes.ToArray();
        }

        public IToken Token { get; }

        public INode[] ConcurrentNodes { get; }
    }
}
