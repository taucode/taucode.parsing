namespace TauCode.Parsing.Tokens
{
    public class IdentifierToken : TokenBase
    {
        #region Constructor

        public IdentifierToken(string identifier)
        {
            this.Identifier = identifier;
        }


        #endregion

        #region Public

        public string Identifier { get; }

        #endregion
    }
}
