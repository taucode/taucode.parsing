using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class SpecialStringNode<TStringClass> : ActionNode where TStringClass : struct
    {
        #region Constructor

        public SpecialStringNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action,
            TStringClass @class)
            : base(family, name, action)
        {
            this.Class = @class;
        }

        #endregion

        #region Overridden

        protected override InquireResult InquireImpl(IToken token, IResultAccumulator resultAccumulator)
        {
            if (
                token is SpecialStringToken<TStringClass> specialStringToken &&
                specialStringToken.Class.Equals(this.Class))
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

        public TStringClass Class { get; }

        #endregion
    }
}