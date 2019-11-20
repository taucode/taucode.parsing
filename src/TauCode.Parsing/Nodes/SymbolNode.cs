using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Nodes
{
    public class SymbolNode : EnumNode<SymbolValue>
    {
        #region Constructor

        public SymbolNode(
            INodeFamily family,
            string name,
            Action<IToken, IResultAccumulator> action)
            : base(family, name, action)
        {
        }


        #endregion
    }
}
