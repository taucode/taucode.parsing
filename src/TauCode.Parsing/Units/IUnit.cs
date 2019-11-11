using System.Collections.Generic;

namespace TauCode.Parsing.Units
{
    public interface IUnit
    {
        string Name { get; set; }
        IBlock Owner { get; }
        bool IsFinalized { get; }
        void FinalizeUnit();
        IReadOnlyList<IUnit> Process(ITokenStream stream, IContext context);
    }
}
