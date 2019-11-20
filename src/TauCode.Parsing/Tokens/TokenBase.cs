namespace TauCode.Parsing.Tokens
{
    public abstract class TokenBase : IToken
    {
        #region Constructor

        protected TokenBase()
        {   
        }

        protected TokenBase(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public

        public string Name { get; set; }

        #endregion
    }
}
