using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface ITokenStream
    {
        IReadOnlyList<IToken> Tokens { get; }
        int Position { get; set; }
        IToken CurrentToken { get; }
    }
}
