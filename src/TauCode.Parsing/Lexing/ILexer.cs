using System.Collections.Generic;

namespace TauCode.Parsing.Lexing
{
    public interface ILexer
    {
        IList<IToken> Lexize(string input);
    }
}
