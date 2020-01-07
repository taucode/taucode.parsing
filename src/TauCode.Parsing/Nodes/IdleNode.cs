using System;

namespace TauCode.Parsing.Nodes
{
    public sealed class IdleNode : NodeImpl
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
            throw new InvalidOperationException("'Inquire' should not be called for 'IdleNode'.");
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new InvalidOperationException("'Act' should not be called for 'IdleNode'.");
        }

        #endregion
    }
}
