using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class IntegerNode : ActionNode
    {
        #region Constructor

        public IntegerNode(
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
        }

        #endregion

        #region Overridden

        protected override bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            return token is IntegerToken;
        }

        #endregion
    }
}
