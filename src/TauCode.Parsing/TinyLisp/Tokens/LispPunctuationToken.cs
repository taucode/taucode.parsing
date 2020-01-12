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
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(value, position, consumedLength, name, properties)
        {
            // todo: check 'consumedLength == 1'?
        }

        public override string ToString() => Value.PunctuationToChar().ToString();
    }
}
