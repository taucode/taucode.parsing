using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class TextProcessingException : ParsingExceptionBase
    {
        public TextProcessingException(string message)
            : base(message)
        {
        }

        public TextProcessingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
