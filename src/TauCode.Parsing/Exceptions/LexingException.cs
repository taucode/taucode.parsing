using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class LexingException : Exception
    {
        public LexingException(string message)
            : base(message)
        {
        }

        public LexingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
