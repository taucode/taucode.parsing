using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class SpecialStringToken : TokenBase
    {
        public SpecialStringToken(
            string @class,
            string value,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Value = value ?? throw new ArgumentNullException(nameof(name));
        }

        public string Class { get; }

        public string Value { get; set; }
    }
}
