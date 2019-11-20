using System;
using TauCode.Utils.CommandLine.Parsing;

namespace TauCode.Parsing.Nodes
{
    public abstract class ActionNode : NodeImpl
    {
        #region Constructor

        protected ActionNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action)
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
                throw new ParsingException("'Act' should not be called if 'Action' is null.");
            }

            this.Action(token, resultAccumulator);
            resultAccumulator.Modify();
        }

        #endregion

        #region Public

        public Action<IToken, IResultAccumulator> Action { get; set; }

        #endregion
    }
}
