using System;
using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class TokenStream : ITokenStream
    {
        private readonly List<IToken> _tokens;
        private int _position;

        public TokenStream(IEnumerable<IToken> tokens)
        {
            _tokens = new List<IToken>(tokens);
        }

        public IReadOnlyList<IToken> Tokens => _tokens;
        public int Position
        {
            get => _position;
            set
            {
                if (value < 0 || value > _tokens.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _position = value;
            }
        }
    }
}
