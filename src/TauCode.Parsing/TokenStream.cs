using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing
{
    public class TokenStream : ITokenStream
    {
        #region Fields

        private readonly List<IToken> _tokens;
        private int _position;

        #endregion

        #region Constructor

        public TokenStream(IEnumerable<IToken> tokens)
        {
            _tokens = new List<IToken>(tokens);
        }

        #endregion

        #region ITokenStream Members

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

        public IToken CurrentToken
        {
            get
            {
                if (this.IsEndOfStream())
                {
                    throw new ParsingException("End of token stream.");
                }

                var token = _tokens[_position];
                return token;

            }
        }


        #endregion
    }
}
