using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Units;

namespace TauCode.Parsing.Aide.Building
{
    // todo: make internal
    public class BuildWorker
    {
        private readonly IAideResult[] _results;
        private readonly IBuildEnvironment _buildEnvironment;
        private readonly List<BlockDefinitionResult> _blockDefinitionResults;
        private readonly List<CloneBlockResult> _cloneBlockResults;

        //private List<>

        public BuildWorker(IAideResult[] results, IBuildEnvironment buildEnvironment)
        {
            _results = results;
            _buildEnvironment = buildEnvironment;

            _blockDefinitionResults = this.ExtractBlockDefinitionResults();
            _cloneBlockResults = this.ExtractCloneBlockResults();

            if (_blockDefinitionResults.Count + _cloneBlockResults.Count != _results.Length)
            {
                throw new NotImplementedException();
            }

            var dictionary = new Dictionary<string, BlockDefinitionResult>();
            foreach (var blockDefinitionResult in _blockDefinitionResults)
            {
                blockDefinitionResult.CheckBlockDefinitionResultIsCorrect();

                var name = blockDefinitionResult.GetBlockDefinitionResultName();
                if (dictionary.ContainsKey(name))
                {
                    throw new NotImplementedException();
                }

                dictionary.Add(name, blockDefinitionResult);
            }

            var graph = new Graph<BlockDefinitionResult>();

            foreach (var blockDefinitionResult in dictionary.Values)
            {
                graph.AddNode(blockDefinitionResult);
            }

            foreach (var node in graph.Nodes)
            {
                var block = node.Value;
                var referencedBlockNames = block.GetReferencedBlockNames();

                foreach (var referencedBlockName in referencedBlockNames)
                {
                    var referencedNode =
                        graph.Nodes.Single(x => x.Value.GetBlockDefinitionResultName() == referencedBlockName); // todo: single-or-default

                    node.DrawEdgeTo(referencedNode);
                }
            }

            // todo: slice & go on.
            throw new NotImplementedException();

            //var graph = new Graph<BlockDefinitionResult>();
            //var algorithm = new GraphSlicingAlgorithm<BlockDefinitionResult>(graph);
            //var kaka = algorithm.Slice();
        }

        private List<CloneBlockResult> ExtractCloneBlockResults()
        {
            return _results
                .Where(x => x is CloneBlockResult)
                .Cast<CloneBlockResult>()
                .ToList();
        }

        private List<BlockDefinitionResult> ExtractBlockDefinitionResults()
        {
            return _results
                .Where(x => x is BlockDefinitionResult)
                .Cast<BlockDefinitionResult>()
                .ToList();
        }

        public IBlock BuildMainBlock()
        {
            throw new NotImplementedException();
        }
    }
}
