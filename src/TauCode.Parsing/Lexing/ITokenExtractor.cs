namespace TauCode.Parsing.Lexing
{
    public interface ITokenExtractor
    {
        TokenExtractionResult Extract(string input, int position);
        bool AllowsFirstChar(char firstChar);
    }
}
