using System;

namespace TauCode.Parsing.Exceptions
{
    // todo: consider renaming it into ~'ParseFailedException'
    [Serializable]
    public class ParsingException : Exception
    {
        public ParsingException(string message)
            : base(message)
        {
        }

        public ParsingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
