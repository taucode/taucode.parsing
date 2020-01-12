using System;

namespace TauCode.Parsing.Tokens
{
    public abstract class TokenBase : IToken
    {
        #region Constructor

        protected TokenBase(
            Position position,
            int consumedLength)
        {
            this.Position = position;
            if (consumedLength <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.ConsumedLength = consumedLength;
        }

        #endregion

        #region IToken Members

        public virtual bool HasPayload => true;
        public Position Position { get; }
        public int ConsumedLength { get; }

        #endregion
    }
}
