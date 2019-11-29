using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class CommentToken : TokenBase
    {
        public CommentToken(
            string comment,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
        {
            this.Comment = comment ?? throw new ArgumentNullException(nameof(comment));
        }

        public string Comment { get; }
    }
}
