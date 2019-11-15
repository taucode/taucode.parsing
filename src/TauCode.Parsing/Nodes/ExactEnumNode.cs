using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class ExactEnumNode<TEnum> : ActionNode where TEnum : struct
    {
        #region Constructor

        public ExactEnumNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action, TEnum value)
            : base(family, name, action)
        {
            this.Value = value;
        }

        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (token is EnumToken<TEnum> enumToken && enumToken.Value.Equals(this.Value))
            {
                return this.Action == null ? InquireResult.Skip : InquireResult.Act;
            }
            else
            {
                return InquireResult.Reject;
            }
        }

        #endregion

        #region Public

        public TEnum Value { get; }

        #endregion
    }
}
