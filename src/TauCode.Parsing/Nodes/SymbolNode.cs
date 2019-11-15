using System;
using TauCode.Parsing.Nodes;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class SymbolNode : EnumNode<SymbolValue>
    {
        public SymbolNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }
    }
}
