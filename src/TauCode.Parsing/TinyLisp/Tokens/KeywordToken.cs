using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    public class KeywordToken : TokenBase
    {
        public KeywordToken(
            string keyword,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Keyword = keyword ?? throw new ArgumentNullException(nameof(keyword));
        }

        public string Keyword { get; }
    }
}
