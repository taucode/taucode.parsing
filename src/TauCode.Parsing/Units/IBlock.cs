using System.Collections.Generic;

namespace TauCode.Parsing.Units
{
    public interface IBlock : IUnit
    {
        IUnit Head { get; }
        void Capture(params IUnit[] units);
        bool Owns(IUnit unit);
        IReadOnlyList<IUnit> Owned { get; }
        INode GetSingleExitNode();
    }
}
