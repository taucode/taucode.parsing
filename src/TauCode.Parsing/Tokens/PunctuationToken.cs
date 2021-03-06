﻿using System;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tokens
{
    public class PunctuationToken : TokenBase
    {
        public PunctuationToken(
            char c,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            if (!LexingHelper.IsStandardPunctuationChar(c))
            {
                throw new ArgumentOutOfRangeException(nameof(c));
            }

            this.Value = c;
        }

        public char Value { get; }

        public override string ToString() => this.Value.ToString();
    }
}
