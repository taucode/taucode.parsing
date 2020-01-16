using System;

namespace TauCode.Parsing.Nodes
{
    // todo clean
    public sealed class IdleNode : NodeImpl
    {
        #region Constructor

        public IdleNode(INodeFamily family, string name)
            : base(family, name)
        {
        }

        #endregion

        #region Overridden

        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new InvalidOperationException($"'{nameof(Inquire)}' should not be called for '{typeof(IdleNode).Name}'.");
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new InvalidOperationException($"'{nameof(Act)}' should not be called for '{typeof(IdleNode).Name}'.");
        }

        #endregion
    }
}
