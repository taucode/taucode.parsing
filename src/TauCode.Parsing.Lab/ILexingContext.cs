using System.Collections.Generic;

namespace TauCode.Parsing.Lab
{
    public interface ILexingContext : ITextProcessingContext
    {
        IReadOnlyList<IToken> Tokens { get; }
    }
}
