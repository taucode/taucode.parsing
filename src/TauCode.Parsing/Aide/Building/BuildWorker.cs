using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Units;
using TauCode.Parsing.Units.Impl.Nodes;

namespace TauCode.Parsing.Aide.Building
{
    // todo: make internal
    public class BuildWorker
    {
        private class BlockRecord
        {
            private IBlock _block;

            public BlockRecord(BlockDefinitionResult mold)
            {
                this.Mold = mold;
            }

            public BlockDefinitionResult Mold { get; }

            public IBlock Block
            {
                get => _block;
                set
                {
                    throw new NotImplementedException();
                }
            }
            public bool BlockIsEmbedded { get; }
        }

        private readonly IAideResult[] _results;
        private readonly IBuildEnvironment _buildEnvironment;
        private readonly Dictionary<string, BlockRecord> _records;
        private readonly List<string> _sortedNames;

        public BuildWorker(IAideResult[] results, IBuildEnvironment buildEnvironment)
        {
            _results = results;
            _buildEnvironment = buildEnvironment;

            var blockDefinitionResults = this.ExtractBlockDefinitionResults();
            var dictionary = new Dictionary<string, BlockDefinitionResult>();
            foreach (var blockDefinitionResult in blockDefinitionResults)
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

            var algorithm = new GraphSlicingAlgorithm<BlockDefinitionResult>(graph);
            var slices = algorithm
                .Slice();

            var firstSlice = slices.First();

            foreach (var node in firstSlice.Nodes)
            {
                if (node.OutgoingEdges.Any() || node.IncomingEdges.Any())
                {
                    throw new NotImplementedException(); // still circular refs
                }
            }

            _sortedNames = slices
                .SelectMany(x => x.Nodes.Select(y => y.Value.GetBlockDefinitionResultName()))
                .ToList();

            _records = dictionary.ToDictionary(x => x.Key, x => new BlockRecord(x.Value));
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
            foreach (var name in _sortedNames)
            {
                var record = _records[name];
                var block = this.BuildBlock(record.Mold);
                record.Block = block;
            }

            throw new NotImplementedException();
        }

        private IBlock BuildBlock(BlockDefinitionResult mold)
        {
            var content = mold.Content;
            var contentTransformation = this.BuildContent(content);

            throw new NotImplementedException();
        }

        private List<IUnit> BuildContent(Content content)
        {
            var list = new List<IUnit>();

            for (var i = 0; i < content.UnitResultCount; i++)
            {
                var unitResult = content[i];
                var unit = this.BuildUnit(unitResult);
                list.Add(unit);

                if (i > 0 && list[i - 1] is INode previousNode)
                {
                    previousNode.AddLink(unit);
                }
            }

            return list;
        }

        private IUnit BuildUnit(UnitResult unitResult)
        {
            IUnit unit;
            if (unitResult is SyntaxElementResult syntaxElementResult)
            {
                switch (syntaxElementResult.SyntaxElement)
                {
                    case SyntaxElement.Identifier:
                        unit = new IdentifierNode(ParsingHelper.IdleTokenProcessor, syntaxElementResult.SourceNodeName);
                        return unit;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
