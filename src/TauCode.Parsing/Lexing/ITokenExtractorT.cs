namespace TauCode.Parsing.Lexing
{
    public interface ITokenExtractor<out TToken> : ITokenExtractor
        where TToken : IToken
    {
        TToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position);
    }
}
