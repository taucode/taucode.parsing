using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class TextToken : TokenBase
    {
        public TextToken(
            ITextClass @class,
            ITextDecoration decoration,
            string text,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Decoration = decoration ?? throw new ArgumentNullException(nameof(decoration));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public ITextClass Class { get; set; }
        public ITextDecoration Decoration { get; set; }
        public string Text { get; set; }
    }
}
