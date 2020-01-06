using System;
using System.Collections.Generic;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tokens
{
    public class PunctuationToken : TokenBase
    {
        public PunctuationToken(
            char c,
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(position, consumedLength, name, properties)
        {
            if (!LexingHelper.IsStandardPunctuationChar(c))
            {
                throw new ArgumentOutOfRangeException(nameof(c));
            }

            this.Value = c;
        }

        public char Value { get; }
    }
}
