using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class ParsingContext : IParsingContext
    {
        #region Fields

        private IReadOnlyCollection<INode> _nodes;

        #endregion

        #region Constructor

        public ParsingContext(ITokenStream tokenStream)
        {
            this.TokenStream = tokenStream ?? throw new ArgumentNullException(nameof(tokenStream));
            this.ResultAccumulator = new ResultAccumulator();
        }

        #endregion

        #region Private



        #endregion

        #region IContext Members

        public ITokenStream TokenStream { get; }

        public void SetNodes(IReadOnlyCollection<INode> nodes)
        {
            _nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
        }

        public IReadOnlyCollection<INode> GetNodes() => _nodes;

        public IResultAccumulator ResultAccumulator { get; }

        #endregion
    }
}
