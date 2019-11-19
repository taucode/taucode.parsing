﻿using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class Boss
    {
        private readonly Dictionary<string, BlockBuilder> _blockBuilders;

        public Boss(IEnumerable<IAideResult> results)
        {
            // todo: checks.

            NodeFamily = new NodeFamily("todo family");

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
        }

        public INode Deliver()
        {
            var orderedNames = this.Squad.GetOrderedNames();
            foreach (var orderedName in orderedNames)
            {
                var blockBuilder = _blockBuilders[orderedName];
                blockBuilder.Build();
            }

            var topNecklaces = _blockBuilders
                .Values
                .Where(x => x.Source.IsTop())
                .Select(x => x.Necklace)
                .ToList();

            if (topNecklaces.Count != 1)
            {
                throw new NotImplementedException(); // todo
            }

            var node = topNecklaces.Single().ToRootNode();
            return node;
        }

        public Squad Squad { get; }

        public INodeFamily NodeFamily { get; }
    }
}
