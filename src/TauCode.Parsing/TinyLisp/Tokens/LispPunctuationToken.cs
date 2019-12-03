using System.Collections.Generic;
using System.Diagnostics;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Tokens
{
    [DebuggerDisplay("{" + nameof(ToString) + "()}")]
    public class LispPunctuationToken : EnumToken<Punctuation>
    {
        public LispPunctuationToken(
            Punctuation value,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(value, name, properties)
        {
        }

        public override string ToString() => Value.PunctuationToChar().ToString();
    }
}
