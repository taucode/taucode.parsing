using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class IntegerToken : TokenBase
    {
        #region Constructor

        public IntegerToken(
            string integerValue,
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(position, consumedLength, name, properties)
        {
            this.Value = integerValue ?? throw new ArgumentNullException(nameof(integerValue));
        }

        #endregion

        #region Public

        public string Value { get; }

        #endregion
    }
}
