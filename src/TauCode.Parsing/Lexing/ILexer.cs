using System.Collections.Generic;

namespace TauCode.Parsing.Lexing
{
    public interface ILexer
    {
        List<IToken> Lexize(string input);
    }
}
