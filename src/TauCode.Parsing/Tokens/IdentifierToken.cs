using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class IdentifierToken : TokenBase
    {
        #region Constructor

        public IdentifierToken(
            string identifier,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        #endregion

        #region Public

        public string Identifier { get; }

        #endregion
    }
}
