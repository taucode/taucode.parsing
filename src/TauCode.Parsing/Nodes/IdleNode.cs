namespace TauCode.Parsing.Nodes
{
    public class IdleNode : NodeImpl
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
