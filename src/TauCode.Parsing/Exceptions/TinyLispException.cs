using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class TinyLispException : Exception
    {
        public TinyLispException(string message)
            : base(message)
        {
        }

        public TinyLispException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
