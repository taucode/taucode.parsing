using TauCode.Parsing.Nodes2;

namespace TauCode.Parsing.Nodes
{
    public class IdleNode : Node2Impl
    {
        public IdleNode(INodeFamily family, string name)
            : base(family, name)
        {
        }

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new System.NotImplementedException();
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new System.NotImplementedException();
        }
    }
}
