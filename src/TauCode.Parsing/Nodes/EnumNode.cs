using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    // todo clean
    public class EnumNode<TEnum> : ActionNode where TEnum : struct
    {
        #region Constructor

        public EnumNode(
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
            var result = token is EnumToken<TEnum>;
            return result;

            //if (token is EnumToken<TEnum>)
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
