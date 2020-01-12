namespace TauCode.Parsing.Old.Lexing
{
    public interface IOldTokenExtractor
    {
        OldTokenExtractionResult Extract(string input, int charIndex, int line, int column);
        bool AllowsFirstChar(char firstChar);
    }
}
