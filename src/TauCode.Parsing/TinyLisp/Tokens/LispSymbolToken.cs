using System;
using System.Collections.Generic;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    [DebuggerDisplay("{" + nameof(SymbolName) + "}")]
    public class LispSymbolToken : TokenBase
    {
        public LispSymbolToken(
            string symbolName,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.SymbolName = symbolName ?? throw new ArgumentNullException(nameof(symbolName));
        }

        public string SymbolName { get; }

        public override string ToString() => SymbolName;
    }
}
