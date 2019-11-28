using System;

namespace TauCode.Parsing.Tokens
{
    public class SpecialStringToken : TokenBase
    {
        public SpecialStringToken(string @class, string value, string name)
            : base(name)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Value = value ?? throw new ArgumentNullException(nameof(name));
        }

        public SpecialStringToken(string @class, string value)
            : this(@class, value, null)
        {
        }

        public string Class { get; }

        public string Value { get; set; }
    }
}
