using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Tokens
{
    public class KeyToken : TokenBase
    {
        internal KeyToken(string fullKeyName)
        {
            if (fullKeyName == null)
            {
                throw new ArgumentNullException(nameof(fullKeyName));
            }

            if (fullKeyName.StartsWith("--"))
            {
                this.Prefix = "--";
                this.KeyName = fullKeyName.Substring(2);
            }
            else if (fullKeyName.StartsWith("-"))
            {
                this.Prefix = "-";
                this.KeyName = fullKeyName.Substring(1);
            }
            else
            {
                throw new NotImplementedException(); // todo: internal error
            }
        }

        public string KeyName { get; }
        public string Prefix { get; }
    }
}
