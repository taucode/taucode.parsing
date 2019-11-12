using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public class SpecialStringNode<TStringClass> : ProcessingNode where TStringClass : struct
    {
        public SpecialStringNode(TStringClass @class, Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
            this.Class = @class;
        }

        public TStringClass Class { get; }

        protected override bool IsAcceptableToken(IToken token)
        {
            return
                token is SpecialStringToken<TStringClass> specialStringToken &&
                specialStringToken.Class.Equals(this.Class);
        }
    }
}
