using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IToken
    {
        string Name { get; }

        bool HasPayload { get; }

        IReadOnlyDictionary<string, string> Properties { get; }
    }
}
