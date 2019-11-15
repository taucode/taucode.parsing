using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Nodes2
{
    public class EndNode : Node2Impl
    {
        public static EndNode Instance = new EndNode();

        private EndNode()
            : base(null, "end")
        {
        }

        protected override InquireResult InquireImpl(IToken token) => InquireResult.End;

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new NotImplementedException(); // error
        }

        public override void AddLink(INode2 node)
        {
            throw new NotImplementedException();
        }

        public override void AddLinkByName(string nodeName)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<INode2> Links => throw new NotImplementedException(); // you should never check this.
    }
}
