using System;

namespace TauCode.Parsing.Aide.Tokens
{
    public class NameReferenceAideToken : AideToken
    {
        public NameReferenceAideToken(string referencedName)
            : base(null)
        {
            this.ReferencedName = referencedName ?? throw new ArgumentNullException(nameof(referencedName));
        }

        public string ReferencedName { get; }
    }
}
