﻿using System;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Units.Impl.Nodes
{
    [DebuggerDisplay("{" + nameof(Value) + "}")]
    public class SymbolNode : ProcessingNode
    {
        private SymbolNode(SymbolValue value, Action<IToken, IContext> processor)
            : base(processor, null)
        {
            this.Value = value;
        }

        public SymbolNode(SymbolValue value, Action<IToken, IContext> processor, string name)
            : base(processor, name)
        {
            this.Value = value;
        }

        private SymbolNode(char c, Action<IToken, IContext> processor)
            : this(LexerHelper.SymbolTokenFromChar(c), processor)
        {
        }

        public SymbolNode(char c, Action<IToken, IContext> processor, string name)
            : this(LexerHelper.SymbolTokenFromChar(c), processor)
        {
            this.Name = name;
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

