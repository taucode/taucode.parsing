using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IResultAccumulator : IReadOnlyList<object>
    {
        void Modify();
        int Version { get; }
        void AddResult(object result);
    }
}
