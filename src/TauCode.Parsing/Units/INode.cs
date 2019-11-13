using System.Collections.Generic;

namespace TauCode.Parsing.Units
{
    public interface INode : IUnit
    {
        void AddLink(IUnit unit);
        void AddLinkAddress(string linkAddress);
        IReadOnlyList<IUnit> Links { get; }
    }
}
