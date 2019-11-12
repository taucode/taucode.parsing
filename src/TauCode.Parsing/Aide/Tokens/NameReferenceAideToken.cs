using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Tokens
{
    public class NameReferenceAideToken : TokenBase
    {
        public NameReferenceAideToken(string referencedName)
        {
            this.ReferencedName = referencedName ?? throw new ArgumentNullException(nameof(referencedName));
        }

        public string ReferencedName { get; }
    }
}
