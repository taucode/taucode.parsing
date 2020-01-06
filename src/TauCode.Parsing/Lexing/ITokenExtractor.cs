namespace TauCode.Parsing.Lexing
{
    public interface ITokenExtractor
    {
        TokenExtractionResult Extract(string input, int charIndex, int line, int column);
        bool AllowsFirstChar(char firstChar);
    }
}
