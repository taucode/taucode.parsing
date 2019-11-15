using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public interface IAideResult2
    {
        string Name { get; }
        IList<string> Arguments { get; }
    }
}
