using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class IntegerNode : ActionNode
    {

        #region Constructor

        public IntegerNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }

        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is IntegerToken)
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }

        #endregion
    }
}
