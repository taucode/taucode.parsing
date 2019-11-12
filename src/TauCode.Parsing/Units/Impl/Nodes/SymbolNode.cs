using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class SymbolNode : ProcessingNode
    {
        public SymbolNode(SymbolValue value, Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
            this.Value = value;
        }

        public SymbolNode(char c, Action<IToken, IContext> processor, string name)
            : this(LexerHelper.SymbolTokenFromChar(c), processor, name)
        {
        }

        public SymbolValue Value { get; }

        protected override bool IsAcceptableToken(IToken token)
        {
            return
                token is SymbolToken symbolToken &&
                this.Value == symbolToken.Value;
        }
    }
}
