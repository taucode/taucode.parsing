using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    public interface IContent : IReadOnlyList<IAideResult2>
    {
        void AddResult(IAideResult2 result);
        void Seal();
        bool IsSealed { get; }
    }
}
