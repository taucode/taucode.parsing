namespace TauCode.Parsing.Tokens
{
    public class NullToken : IToken
    {
        public static NullToken Instance { get; } = new NullToken();

        private NullToken()
        {   
        }

        public Position Position => Position.Zero;
        public int ConsumedLength => -1;
    }
}
