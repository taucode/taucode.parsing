using System;

namespace TauCode.Parsing.Tokens
{
    public class IntegerToken : TokenBase
    {
        #region Constructor

        public IntegerToken(
            string integerValue,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Value = integerValue ?? throw new ArgumentNullException(nameof(integerValue));
        }

        #endregion

        #region Public

        public string Value { get; }

        #endregion

        #region Overridden

        public override string ToString() => this.Value;

        #endregion
    }
}
