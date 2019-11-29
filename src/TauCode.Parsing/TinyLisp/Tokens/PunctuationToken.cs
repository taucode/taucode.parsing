using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    public class PunctuationToken : EnumToken<Punctuation>
    {
        public PunctuationToken(
            Punctuation value,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(value, name, properties)
        {
        }
    }
}
