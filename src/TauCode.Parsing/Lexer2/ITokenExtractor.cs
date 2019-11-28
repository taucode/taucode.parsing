namespace TauCode.Parsing.Lexer2
{
    public interface ITokenExtractor
    {
        TokenExtractionResult Extract(string input, int position);
        bool AllowsFirstChar(char firstChar);
    }
}
