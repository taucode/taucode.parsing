using System.Collections.Generic;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public interface ILexingContext : ITextProcessingContext
    {
        IReadOnlyList<IToken> Tokens { get; }
    }
}
