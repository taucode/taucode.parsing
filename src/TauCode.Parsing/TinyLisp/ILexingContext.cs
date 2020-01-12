using System.Collections.Generic;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.TinyLisp
{
    public interface ILexingContext : ITextProcessingContext
    {
        IReadOnlyList<IToken> Tokens { get; }
    }
}
