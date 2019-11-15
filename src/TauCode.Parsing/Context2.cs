using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class Context2 : IContext2
    {
        private IReadOnlyCollection<INode2> _nodes;

        public Context2(ITokenStream tokenStream)
        {
            this.TokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
            this.ResultAccumulator = new ResultAccumulator();
        }

        public ITokenStream TokenStream { get; }

        public void SetNodes(IReadOnlyCollection<INode2> nodes)
        {
            // todo: checks

            _nodes = nodes;
        }

        public IReadOnlyCollection<INode2> GetNodes() => _nodes;

        public IResultAccumulator ResultAccumulator { get; }
    }
}
