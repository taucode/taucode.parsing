using System;

namespace TauCode.Parsing.Aide.Tokens
{
    public class NameReferenceAideToken : AideToken
    {
        public NameReferenceAideToken(string referencedTokenName)
            : base(null)
        {
            this.ReferencedTokenName = referencedTokenName ?? throw new ArgumentNullException(nameof(referencedTokenName));
        }

        public string ReferencedTokenName { get; }
    }
}
