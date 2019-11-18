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

        public NodeBuilder SubTree { get; private set; }

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
            throw new NotImplementedException();
        }

        //public void Build()
        //{
        //    var outcome = this.CreateContentOutcome(this.Source.Content);
        //    throw new NotImplementedException();
        //}

        //private ContentOutcome CreateContentOutcome(IContent content)
        //{
        //    var outcome = new ContentOutcome();

        //    foreach (var aideResult in content)
        //    {
        //        if (aideResult is TokenResult tokenResult)
        //        {
        //            var nodeBuilder = new NodeBuilder(this.Boss.NodeFamily, tokenResult);
        //            nodeBuilder.Build();
        //            outcome.AddNode(nodeBuilder, true);
        //        }
        //        else if (aideResult is OptionalResult optionalResult)
        //        {
        //            var start = new NodeBuilder(this.Boss.NodeFamily, optionalResult.Name);
        //            var optionalOutcome = this.CreateContentOutcome(optionalResult.OptionalContent);
        //            var end = new NodeBuilder(this.Boss.NodeFamily, (string)null);

        //            throw new NotImplementedException();
        //        }
        //        else if (aideResult is AlternativesResult alternativesResult)
        //        {
        //            var start = new NodeBuilder(this.Boss.NodeFamily, alternativesResult.Name);

        //            var alternativeOutcomes = alternativesResult
        //                .GetAllAlternatives()
        //                .Select(x => this.CreateContentOutcome(x))
        //                .ToList();

        //            var end = new NodeBuilder(this.Boss.NodeFamily, (string)null);

        //            foreach (var alternativeOutcome in alternativeOutcomes)
        //            {
        //                start.Node.EstablishLink(alternativeOutcome.Nodes.First().Node);
        //                alternativeOutcome.AddNode(end, true);
        //            }


        //        }
        //        else
        //        {
        //            throw new NotImplementedException();
        //        }
        //    }

        //    //outcome.InitLinks();

        //    return outcome;
        //}
    }
}
