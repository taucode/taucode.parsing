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
            Position position,
            int consumedLength)
            : base(position, consumedLength)
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
