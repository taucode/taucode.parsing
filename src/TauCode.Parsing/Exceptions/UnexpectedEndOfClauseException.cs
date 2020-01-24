using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class UnexpectedEndOfClauseException : ParseClauseFailedException
    {
        public UnexpectedEndOfClauseException(object[] partialParsingResults)
            : base("Unexpected end of clause.", partialParsingResults)
        {
        }
    }
}
