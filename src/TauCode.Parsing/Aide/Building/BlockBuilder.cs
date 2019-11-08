using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Building
{
    public class BlockBuilder : BuilderBase
    {
        private readonly List<NodeBuilder> _nodes;

        public BlockBuilder()
        {
            _nodes = new List<NodeBuilder>();
        }

        public string Name { get; set; }
        public IReadOnlyList<NodeBuilder> Nodes => _nodes;

        public void AddNode(NodeBuilder node)
        {
            throw new NotImplementedException();
        }
    }
}
