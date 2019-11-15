using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    public interface IAideResult2
    {
        string Name { get; }
        IList<string> Arguments { get; }
    }
}
