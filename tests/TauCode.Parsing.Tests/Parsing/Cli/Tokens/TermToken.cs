using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Tokens
{
    public class TermToken : TokenBase
    {
        internal TermToken(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value cannot be empty.", nameof(value));
            }

            this.Value = value;
        }

        public string Value { get; }
    }
}
