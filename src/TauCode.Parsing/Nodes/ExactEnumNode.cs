using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    // todo claen
    public class ExactEnumNode<TEnum> : ActionNode where TEnum : struct
    {
        #region Constructor

        public ExactEnumNode(
            TEnum value,
            Action<ActionNode, IToken, IResultAccumulator> action,
            INodeFamily family,
            string name)
            : base(action, family, name)
        {
            this.Value = value;
        }

        #endregion

        #region Overridden

        protected override /*InquireResult*/ bool InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            var result =
                token is EnumToken<TEnum> enumToken &&
                enumToken.Value.Equals(this.Value);

            return result;

            //if (token is EnumToken<TEnum> enumToken && enumToken.Value.Equals(this.Value))
            //{
            //    return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            //}
            //else
            //{
            //    return InquireResult.Reject;
            //}
        }

        #endregion

        #region Public

        public TEnum Value { get; }

        #endregion
    }
}
