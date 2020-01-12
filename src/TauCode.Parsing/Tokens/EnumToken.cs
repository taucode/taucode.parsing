using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class EnumToken<TEnum> : TokenBase where TEnum : struct
    {
        #region Constructor

        public EnumToken(
            TEnum value,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Value = value;
        }

        #endregion

        #region Public

        public TEnum Value { get; }

        #endregion
    }
}
