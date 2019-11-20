using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class ParserException : Exception
    {
        public ParserException(string message)
            : base(message)
        {
        }

        public ParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
