using System;

namespace TauCode.Parsing.Tokens
{
    public class WordToken : TokenBase
    {
        #region Constructors

        public WordToken(string word)
            : this(word, null)
        {
        }

        public WordToken(string word, string name)
            : base(name)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        #endregion

        #region Public

        public string Word { get; }

        #endregion
    }
}
