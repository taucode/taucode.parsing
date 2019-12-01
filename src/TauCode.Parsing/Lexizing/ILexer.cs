using System.Collections.Generic;

namespace TauCode.Parsing.Lexizing
{
    public interface ILexer
    {
        List<IToken> Lexize(string input);
    }
}
