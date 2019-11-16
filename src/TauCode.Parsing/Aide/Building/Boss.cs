using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class Boss
    {
        private readonly Dictionary<string, BlockBuilder> _blockBuilders;
        private INode _root;

        public Boss(IEnumerable<IAideResult> results)
        {
            // todo: checks.
            //_blockDefinitions = results.Cast<BlockDefinitionResult>().ToList();

            this.Squad = new Squad();
            _blockBuilders = results
                .Cast<BlockDefinitionResult>()
                .Select(x => new BlockBuilder(this, x))
                .ToDictionary(x => x.Source.GetBlockName(), x => x);

            this.Resolve();
        }

        private void Resolve()
        {
            foreach (var blockBuilder in _blockBuilders.Values)
            {
                blockBuilder.Resolve();
            }

            var orderedNames = this.Squad.GetOrderedNames();
            foreach (var orderedName in orderedNames)
            {
                var blockBuilder = _blockBuilders[orderedName];
                blockBuilder.Build();
            }

            throw new NotImplementedException();
        }

        public INode Deliver()
        {
            throw new System.NotImplementedException();
        }

        public Squad Squad { get; }
    }
}
