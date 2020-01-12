using System;
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
            int consumedLength)
            : base(value, position, consumedLength)
        {
            if (consumedLength != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(consumedLength));
            }
        }

        public override string ToString() => Value.PunctuationToChar().ToString();
    }
}
