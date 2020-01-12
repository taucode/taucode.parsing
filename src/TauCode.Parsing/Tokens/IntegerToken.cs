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
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Value = integerValue ?? throw new ArgumentNullException(nameof(integerValue));
        }

        #endregion

        #region Public

        public string Value { get; }

        #endregion
    }
}
