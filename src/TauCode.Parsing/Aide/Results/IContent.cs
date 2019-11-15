using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public interface IContent : IReadOnlyList<IAideResult>
    {
        IAideResult Owner { get; }
        void AddResult(IAideResult result);
        void Seal();
        bool IsSealed { get; }
    }
}
