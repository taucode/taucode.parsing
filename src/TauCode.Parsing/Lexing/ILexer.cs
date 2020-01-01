using System.Collections.Generic;

namespace TauCode.Parsing.Lexing
{
    public interface ILexer
    {
        ILexingEnvironment Environment { get; }

        IList<IToken> Lexize(string input);
    }
}
