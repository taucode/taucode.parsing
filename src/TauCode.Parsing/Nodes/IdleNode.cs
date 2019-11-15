namespace TauCode.Parsing.Nodes
{
    public class IdleNode : NodeImpl
    {
        #region Constructor

        public IdleNode(INodeFamily family, string name)
            : base(family, name)
        {
        }


        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new System.NotImplementedException();
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new System.NotImplementedException();
        }


        #endregion
    }
}
