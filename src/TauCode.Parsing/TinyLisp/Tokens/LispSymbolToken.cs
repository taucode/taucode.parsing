using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    public class LispSymbolToken : TokenBase
    {
        public LispSymbolToken(
            string symbol,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        public string Symbol { get; }
    }
}
