using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public interface IParsingBlock : IParsingUnit
    {
        IParsingUnit Head { get; }
        void Add(params IParsingUnit[] units);
        bool Owns(IParsingUnit unit);
        IReadOnlyList<IParsingUnit> Owned { get; }
    }
}
