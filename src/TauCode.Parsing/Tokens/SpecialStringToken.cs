namespace TauCode.Parsing.Tokens
{
    public class SpecialStringToken<TStringClass> : TokenBase where TStringClass : struct
    {
        #region Constructor

        public SpecialStringToken(TStringClass @class, string value)
        {
            this.Class = @class;
            this.Value = value;
        }

        #endregion

        #region Public

        public TStringClass Class { get; }

        public string Value { get; }

        #endregion
    }
}
