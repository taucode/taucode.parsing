using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    // todo clean
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

        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            return token is IntegerToken;
            //if (token is IntegerToken)
            //{
            //    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            //}
            //else
            //{
            //    return InquireResult.Reject;
            //}
        }

        #endregion
    }
}
