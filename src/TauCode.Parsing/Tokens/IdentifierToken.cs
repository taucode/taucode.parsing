namespace TauCode.Parsing.Tokens
{
    public class IdentifierToken : TokenBase
    {
        public IdentifierToken(string identifier)
        {
            this.Identifier = identifier;
        }

        public string Identifier { get; }
    }
}
