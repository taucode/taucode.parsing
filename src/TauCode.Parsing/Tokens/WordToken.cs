using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class WordToken : TokenBase
    {
        #region Constructor

        public WordToken(
            string word,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        #endregion

        #region Public

        public string Word { get; }

        #endregion
    }
}
