﻿using System;
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
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.SymbolName = symbolName ?? throw new ArgumentNullException(nameof(symbolName));
        }

        public string SymbolName { get; }

        public override string ToString() => SymbolName;
    }
}
