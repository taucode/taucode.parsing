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
            IList<INode> concurrentNodes,
            object[] partialParsingResults)
            : base("More than one node accepted the token.", partialParsingResults)
        {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));

            if (concurrentNodes == null)
            {
                throw new ArgumentNullException(nameof(concurrentNodes));
            }

            if (!concurrentNodes.Any())
            {
                throw new ArgumentException($"'{concurrentNodes}' cannot be empty.");
            }

            this.ConcurrentNodes = concurrentNodes.ToArray();
        }

        public IToken Token { get; }

        public INode[] ConcurrentNodes { get; }
    }
}
