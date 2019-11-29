namespace TauCode.Parsing.Lexer2
{
    public struct TokenExtractionResult
    {
        public TokenExtractionResult(int shift, IToken token)
        {
            this.Shift = shift;
            this.Token = token;
        }

        public IToken Token { get; }
        public int Shift { get; }
    }
}
