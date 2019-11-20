using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public interface IAideResult
    {
        string Name { get; }
        List<string> Arguments { get; }
    }
}
