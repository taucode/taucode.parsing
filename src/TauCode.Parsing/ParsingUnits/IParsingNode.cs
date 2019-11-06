using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public interface IParsingNode : IParsingUnit
    {
        void AddLink(IParsingUnit linked);
        IReadOnlyList<IParsingUnit> Links { get; }
    }
}
