using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class BlockBuilder
    {
        public BlockBuilder(Boss boss, BlockDefinitionResult source)
        {
            this.Boss = boss ?? throw new ArgumentNullException(nameof(boss));
            this.Source = source ?? throw new ArgumentNullException(nameof(source));
            this.GraphNode = this.Boss.Squad.RegisterBuilder(this);

            this.ReferencedBlockNames = this
                .Source
                .Content
                .GetAllTokenResultsFromContent()
                .Select(x => x.Token)
                .Where(x =>
                    x is EnumToken<SyntaxElement> syntaxEnumToken &&
                    syntaxEnumToken.Value == SyntaxElement.BlockReference)
                .Cast<EnumToken<SyntaxElement>>()
                .Select(x => x.Name)
                .ToList();
        }

        public Boss Boss { get; }
        public Node<BlockBuilder> GraphNode { get; }
        public BlockDefinitionResult Source { get; }
        public List<string> ReferencedBlockNames { get; }

        public Necklace Necklace { get; private set; }

        internal void Resolve()
        {
            foreach (var referencedBlockName in this.ReferencedBlockNames)
            {
                var referencedBlockBuilder = this.Boss.Squad.GetBlockBuilder(referencedBlockName);
                this.GraphNode.DrawEdgeTo(referencedBlockBuilder.GraphNode);
            }
        }

        public void Build()
        {
            if (this.Necklace != null)
            {
                throw new AideException("Block is already built.");
            }

            this.Necklace = this.ContentToNecklace(this.Source.Content);
        }

        private Necklace ContentToNecklace(IContent content)
        {
            var necklace = new Necklace();

            for (var i = 0; i < content.Count; i++)
            {
                var result = content[i];
                if (result is TokenResult tokenResult)
                {
                    if (tokenResult.Token is EnumToken<SyntaxElement> syntaxToken &&
                        syntaxToken.Value == SyntaxElement.BlockReference)
                    {
                        var referencedBlockName = syntaxToken.Name;
                        var blockNecklace = this.Boss.Squad.GetBlockBuilder(referencedBlockName).Necklace;
                        if (blockNecklace == null)
                        {
                            throw new AideException($"Necklace not built for block '{referencedBlockName}'.");
                        }

                        blockNecklace.Right.Arguments.AddRange(tokenResult.Arguments);

                        // add idle node with name == <name of block> in front of the block
                        var blockStartingItem = new NodeBuilder(new IdleNode(this.Boss.NodeFamily, referencedBlockName), null);
                        necklace.AddItem(blockStartingItem, true);
                        
                        // append block necklace
                        necklace.AppendNecklace(blockNecklace);
                    }
                    else
                    {
                        var nodeBuilder = new NodeBuilder(this.Boss.NodeFamily, tokenResult);
                        necklace.AddItem(nodeBuilder, true);
                    }
                }
                else if (result is OptionalResult optionalResult)
                {
                    var optionalNecklace = this.BuildOptionalNecklace(optionalResult);
                    optionalNecklace.Right.Arguments.AddRange(optionalResult.Arguments);
                    necklace.AppendNecklace(optionalNecklace);
                }
                else if (result is AlternativesResult alternativesResult)
                {
                    var alternativesNecklace = this.BuildAlternativesNecklace(alternativesResult);
                    alternativesNecklace.Right.Arguments.AddRange(alternativesResult.Arguments);
                    necklace.AppendNecklace(alternativesNecklace);
                }
                else
                {
                    throw new AideException("Unexpected result.");
                }
            }

            return necklace;
        }

        private Necklace BuildOptionalNecklace(OptionalResult optionalResult)
        {
            var result = new Necklace();
            var left = new NodeBuilder(new IdleNode(this.Boss.NodeFamily, optionalResult.Name), null);
            result.AddItem(left, false);
            var right = new NodeBuilder(new IdleNode(this.Boss.NodeFamily, null), optionalResult.Arguments);
            result.AddItem(right, true);

            var optionalContentNecklace = this.ContentToNecklace(optionalResult.OptionalContent);
            
            result.InsertNecklace(left, right, optionalContentNecklace);
            return result;
        }

        private Necklace BuildAlternativesNecklace(AlternativesResult alternativesResult)
        {
            var result = new Necklace();
            var left = new NodeBuilder(new IdleNode(this.Boss.NodeFamily, alternativesResult.Name), null);
            result.AddItem(left, false);
            var right = new NodeBuilder(new IdleNode(this.Boss.NodeFamily, null), alternativesResult.Arguments);
            result.AddItem(right, false);

            foreach (var alternative in alternativesResult.GetAllAlternatives())
            {
                var alternativeNecklace = this.ContentToNecklace(alternative);
                result.InsertNecklace(left, right, alternativeNecklace);
            }

            return result;
        }
    }
}
