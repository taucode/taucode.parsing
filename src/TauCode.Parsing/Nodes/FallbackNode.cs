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

        public Func<FallbackNode, IToken, IResultAccumulator, bool> FallbackPredicate { get; private set; }

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
            throw new NotImplementedException(); // todo error. fallback won't accept links
        }

        public override void EstablishLink(INode node)
        {
            throw new NotImplementedException(); // todo error. fallback won't accept links
        }
    }

    // todo separate file
    public class FallbackNodeAcceptedTokenException : ParsingExceptionBase
    {
        public FallbackNodeAcceptedTokenException(
            FallbackNode fallbackNode,
            IToken token,
            IResultAccumulator resultAccumulator)
            : base("Fallback node accepted token.")
        {
            this.FallbackNode = fallbackNode;
            this.Token = token;
            this.ResultAccumulator = resultAccumulator;
        }

        public FallbackNode FallbackNode { get; }

        public IToken Token { get; }

        public IResultAccumulator ResultAccumulator { get; }
    }
}
