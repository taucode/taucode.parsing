namespace TauCode.Parsing.Lab
{
    public interface IGammaTokenExtractor<out TToken> : IGammaTokenExtractor
        where TToken : IToken
    {
        TToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position);
    }
}
