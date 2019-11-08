using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public interface IParsingBlock : IParsingUnit
    {
        IParsingUnit Head { get; }
        void Capture(params IParsingUnit[] units);
        bool Owns(IParsingUnit unit);
        IReadOnlyList<IParsingUnit> Owned { get; }
        IParsingNode GetSingleExitNode();
    }
}
