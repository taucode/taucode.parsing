using System;

namespace TauCode.Parsing.Nodes
{
    public sealed class FallbackNode : NodeImpl
    {
        public FallbackNode(INodeFamily family, string name) : base(family, name)
        {
        }


        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException();
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
}
