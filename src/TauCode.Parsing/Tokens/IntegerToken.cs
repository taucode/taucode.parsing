using System;

namespace TauCode.Parsing.Tokens
{
    public class IntegerToken : TokenBase
    {
        #region Constructors

        public IntegerToken(string integerValue)
            : this(integerValue, null)
        {

        }

        public IntegerToken(string integerValue, string name)
            : base(name)
        {
            this.IntegerValue = integerValue ?? throw new ArgumentNullException(nameof(integerValue));
        }

        #endregion

        #region Public

        public string IntegerValue { get; }

        #endregion
    }
}
