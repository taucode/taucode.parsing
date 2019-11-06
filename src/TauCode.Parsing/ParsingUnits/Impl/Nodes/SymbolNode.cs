using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.ParsingUnits.Impl.Nodes
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class SymbolNode : ParsingNode
    {
        public SymbolNode(SymbolValue value, Action<IToken, IParsingContext> processor)
            : base(processor)
        {
            this.Value = value;
        }

        public SymbolNode(char c, Action<IToken, IParsingContext> processor)
            : this(LexerHelper.SymbolTokenFromChar(c), processor)
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

