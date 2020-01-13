using System;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Old.Tokens
{
    public class OldCommentToken : TokenBase
    {
        public OldCommentToken(
            string comment,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public string Comment { get; }
    }
}
