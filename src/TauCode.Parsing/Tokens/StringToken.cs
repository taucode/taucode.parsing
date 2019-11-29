using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class StringToken : TokenBase
    {
        #region Constructor

        public StringToken(
            string value,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        #endregion

        #region Public

        public string Value { get; }

        #endregion
    }
}
