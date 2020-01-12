using System;
using System.Collections.Generic;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Old.Tokens
{
    public class OldTextToken : TokenBase
    {
        public OldTextToken(
            IOldTextClass @class,
            IOldTextDecoration decoration,
            string text,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Class = @class ?? throw new ArgumentNullException(nameof(@class));
            this.Decoration = decoration ?? throw new ArgumentNullException(nameof(decoration));
            this.Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public IOldTextClass Class { get; }
        public IOldTextDecoration Decoration { get; }
        public string Text { get; }
    }
}
