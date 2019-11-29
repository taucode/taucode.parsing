using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class StringToken : TokenBase
    {
        #region Constructor

        public StringToken(
            string str,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.String = str ?? throw new ArgumentNullException(nameof(str));
        }

        #endregion

        #region Public

        public string String { get; }

        #endregion
    }
}
