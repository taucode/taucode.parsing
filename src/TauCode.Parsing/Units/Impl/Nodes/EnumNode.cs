using System;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    public class EnumNode<TEnum> : ProcessingNode where TEnum : struct
    {
        public EnumNode(Action<IToken, IContext> processor, string name) : base(processor, name)
        {
        }

        protected override bool IsAcceptableToken(IToken token)
        {
            throw new NotImplementedException();
        }
    }
}
