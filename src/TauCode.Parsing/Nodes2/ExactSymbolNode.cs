using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes2
{
    public class ExactSymbolNode : ExactEnumNode<SymbolValue>
    {
        public ExactSymbolNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action,
            SymbolValue value)
            : base(family, name, action, value)
        {
        }

        public ExactSymbolNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action,
            char c)
            : base(family, name, action, LexerHelper.SymbolTokenFromChar(c))
        {
        }
    }
}
