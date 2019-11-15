using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class SpecialStringNode<TStringClass> : ActionNode where TStringClass : struct
    {
        public SpecialStringNode(INodeFamily family, string name, Action<IToken, IResultAccumulator> action,
            TStringClass @class)
            : base(family, name, action)
        {
            this.Class = @class;
        }

        public TStringClass Class { get; }

        protected override InquireResult InquireImpl(IToken token)
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
    }
}
