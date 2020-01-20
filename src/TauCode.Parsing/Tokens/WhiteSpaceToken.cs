namespace TauCode.Parsing.Tokens
{
    public class WhiteSpaceToken : TokenBase, IEmptyToken
    {
        public WhiteSpaceToken(Position position, int consumedLength)
            : base(position, consumedLength)
        {
        }
    }
}
