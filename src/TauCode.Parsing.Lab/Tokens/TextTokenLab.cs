using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lab.Tokens
{
    public class TextTokenLab : TokenBase
    {
        public TextTokenLab(
            ITextClass @class,
            ITextDecoration decoration,
            string text,
            Position position,
            int consumedLength,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(position, consumedLength, name, properties)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Decoration = decoration ?? throw new ArgumentNullException(nameof(decoration));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public ITextClass Class { get; }
        public ITextDecoration Decoration { get; }
        public string Text { get; }
    }
}
