using System.Collections.Generic;

namespace TauCode.Parsing.Units
{
    // todo: name cannot be an Aide reserved word!
    public interface IUnit
    {
        string Name { get; }
        IBlock Owner { get; }
        bool IsFinalized { get; }
        void FinalizeUnit();
        IReadOnlyList<IUnit> Process(ITokenStream stream, IContext context);
    }
}
