using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class CommentToken : TokenBase
    {
        public CommentToken(
            string comment,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public string Comment { get; }

        public override bool HasPayload => false;
    }
}
