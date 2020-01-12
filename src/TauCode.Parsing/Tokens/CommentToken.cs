using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    // todo: use textToken with class 'Comment'?
    public class CommentToken : TokenBase
    {
        public CommentToken(
            string comment,
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(position, consumedLength, name, properties)
        {
            this.Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public string Comment { get; }

        public override bool HasPayload => false;
    }
}
