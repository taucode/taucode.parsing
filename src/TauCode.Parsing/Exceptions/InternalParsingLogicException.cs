using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class InternalParsingLogicException : ParsingExceptionBase
    {
        public InternalParsingLogicException(string message)
            : base(message)
        {
        }
    }
}
