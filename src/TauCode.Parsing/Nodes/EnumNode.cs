using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
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

        protected override bool AcceptsTokenImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            var result = token is EnumToken<TEnum>;
            return result;
        }

        #endregion
    }
}
