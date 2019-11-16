using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Algorithms.Graphs;
using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Building
{
    public class BlockBuilder
    {
        private class ContentOutcome
        {
            public INode Root { get; }
            public List<INode> Nodes { get; }
        }

        public BlockBuilder(Boss boss, BlockDefinitionResult source)
        {
            // todo checks
            this.Boss = boss;
            this.Source = source;
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
            var outcome = this.CreateContentOutcome(this.Source.Content);
            throw new NotImplementedException();
        }

        private ContentOutcome CreateContentOutcome(IContent content)
        {
            var outcome = new ContentOutcome();
            var gotRoot = false;

            foreach (var aideResult in this.Source.Content)
            {
                INode node;

                if (aideResult is TokenResult tokenResult)
                {
                    node = this.BuildNode(tokenResult);
                    throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }

                throw new NotImplementedException();
            }

            return outcome;
        }

        private INode BuildNode(TokenResult tokenResult)
        {
            throw new NotImplementedException();
        }
    }
}
