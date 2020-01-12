using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class LexingException : ParsingExceptionBase
    {
        public LexingException(string message, Position? position)
            : base(message)
        {
            this.Position = position;
        }

        public LexingException(string message, Exception innerException, Position? position)
            : base(message, innerException)
        {
            this.Position = position;
        }

        public Position? Position { get; }
    }
}
