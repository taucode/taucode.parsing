using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class TinyLispException : ParsingExceptionBase
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
