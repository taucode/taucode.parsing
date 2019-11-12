namespace TauCode.Parsing.Tokens
{
    public class SpecialStringToken<TStringClass> : TokenBase where TStringClass : struct
    {
        public SpecialStringToken(TStringClass @class, string value)
        {
            this.Class = @class;
            this.Value = value;
        }

        public TStringClass Class { get; }

        public string Value { get; }
    }
}
