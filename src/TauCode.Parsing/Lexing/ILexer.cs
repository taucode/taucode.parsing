using System.Collections.Generic;

namespace TauCode.Parsing.Lexing
{
    public interface ILexer
    {
        ILexingEnvironment Environment { get; }

        List<IToken> Lexize(string input);
    }
}
