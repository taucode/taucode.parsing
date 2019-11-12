namespace TauCode.Parsing.Tokens
{
    public abstract class TokenBase : IToken
    {
        protected TokenBase()
        {   
        }

        protected TokenBase(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
