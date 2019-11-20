using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;

namespace TauCode.Parsing.Aide.Building
{
    public class Squad
    {
        private readonly Dictionary<string, BlockBuilder> _blockBuilders;
        private readonly Graph<BlockBuilder> _graph;

        public Squad()
        {
            _blockBuilders = new Dictionary<string, BlockBuilder>();
            _graph = new Graph<BlockBuilder>();
        }

        public BlockBuilder GetBlockBuilder(string blockName)
        {
            if (blockName == null)
            {
                throw new ArgumentNullException(nameof(blockName));
            }

            return _blockBuilders[blockName];
        }

        public Node<BlockBuilder> RegisterBuilder(BlockBuilder blockBuilder)
        {
            _blockBuilders.Add(blockBuilder.Source.GetBlockName(), blockBuilder);
            var node = _graph.AddNode(blockBuilder);
            return node;
        }

        internal List<string> GetOrderedNames()
        {
            var algorithm = new GraphSlicingAlgorithm<BlockBuilder>(_graph);
            var result = algorithm
                .Slice()
                .SelectMany(x => x.Nodes)
                .Select(x => x.Value.Source.GetBlockName())
                .ToList();

            return result;
        }
    }
}
