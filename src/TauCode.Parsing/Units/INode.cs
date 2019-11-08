using System.Collections.Generic;

namespace TauCode.Parsing.Units
{
    public interface INode : IUnit
    {
        void AddLink(IUnit linked);
        IReadOnlyList<IUnit> Links { get; }
    }
}
