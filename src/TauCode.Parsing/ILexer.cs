using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface ILexer
    {
        List<IToken> Lexize(string input);
    }
}
