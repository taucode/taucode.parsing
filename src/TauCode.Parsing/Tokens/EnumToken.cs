namespace TauCode.Parsing.Tokens
{
    public class EnumToken<TEnum> : TokenBase where TEnum : struct
    {
        public EnumToken(TEnum value)
        {
            this.Value = value;
        }

        public EnumToken(TEnum value, string name)
            : base(name)
        {
            this.Value = value;
        }

        public TEnum Value { get; }
    }
}
