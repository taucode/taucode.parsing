using System;

namespace TauCode.Parsing.Nodes
{
    public abstract class ActionNode : NodeImpl
    {
        #region Constructor

        protected ActionNode(
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(family, name)
        {
            this.Action = action; // can be null
        }

        #endregion

        #region Overridden

        protected override void ActImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (this.Action == null)
            {
                throw new NullReferenceException("'Act' should not be called if 'Action' is null.");
            }

            this.Action(this, token, resultAccumulator);
            resultAccumulator.Modify();
        }

        #endregion

        #region Public

        public Action<ActionNode, IToken, IResultAccumulator> Action { get; set; }

        #endregion
    }
}
