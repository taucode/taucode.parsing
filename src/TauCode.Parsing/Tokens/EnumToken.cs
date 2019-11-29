using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class EnumToken<TEnum> : TokenBase where TEnum : struct
    {
        #region Constructor

        public EnumToken(
            TEnum value,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Value = value;
        }

        #endregion

        #region Public

        public TEnum Value { get; }

        #endregion
    }
}
