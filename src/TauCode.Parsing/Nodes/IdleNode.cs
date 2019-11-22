using TauCode.Parsing.Exceptions;

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
            throw new ParserException("'Inquire' should not be called for 'IdleNode'.");
        }

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            throw new ParserException("'Act' should not be called for 'IdleNode'.");
        }

        #endregion
    }
}
