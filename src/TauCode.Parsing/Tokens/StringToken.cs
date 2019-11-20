using System;

namespace TauCode.Parsing.Tokens
{
    public class StringToken : TokenBase
    {
        #region Constructors

        public StringToken(string str)
            : this(str, null)
        {
        }

        public StringToken(string str, string name)
            : base(name)
        {
            this.String = str ?? throw new ArgumentNullException(nameof(str));
        }

        #endregion

        #region Public

        public string String { get; }

        #endregion
    }
}
