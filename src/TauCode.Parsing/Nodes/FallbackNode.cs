using System;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Nodes
{
    public sealed class FallbackNode : NodeImpl
    {
        public FallbackNode(
            Func<FallbackNode, IToken, IResultAccumulator, bool> fallbackPredicate,
            INodeFamily family,
            string name)
            : base(family, name)
        {
            this.FallbackPredicate = fallbackPredicate ?? throw new ArgumentNullException(nameof(fallbackPredicate));
        }

        public Func<FallbackNode, IToken, IResultAccumulator, bool> FallbackPredicate { get; }

        protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            return this.FallbackPredicate(this, token, resultAccumulator);
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new FallbackNodeAcceptedTokenException(this, token, resultAccumulator);
        }

        public override void ClaimLink(string nodeName)
        {
            throw new InvalidOperationException($"Cannot call '{nameof(ClaimLink)}' for fallback node.");
        }

        public override void EstablishLink(INode node)
        {
            throw new InvalidOperationException($"Cannot call '{nameof(EstablishLink)}' for fallback node.");
        }
    }
}
