using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class NullToken : IToken
    {
        private static readonly Dictionary<string, string> EmptyDictionary = new Dictionary<string, string>();

        public static NullToken Instance { get; } = new NullToken();

        private NullToken()
        {
        }

        public string Name => "<Null>";
        public IReadOnlyDictionary<string, string> Properties => EmptyDictionary;
    }
}
