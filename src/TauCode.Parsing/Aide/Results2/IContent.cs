using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    public interface IContent : IReadOnlyList<IAideResult2>
    {
        IAideResult2 Owner { get; }
        void AddResult(IAideResult2 result);
        void Seal();
        bool IsSealed { get; }
    }
}
