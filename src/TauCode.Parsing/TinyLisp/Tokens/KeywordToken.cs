using System;
using System.Collections.Generic;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    [DebuggerDisplay("{" + nameof(Keyword) + "}")]
    public class KeywordToken : TokenBase
    {
        public KeywordToken(
            string keyword,
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(position, consumedLength, name, properties)
        {
            this.Keyword = keyword ?? throw new ArgumentNullException(nameof(keyword));
        }

        public string Keyword { get; }

        public override string ToString() => Keyword;
    }
}
