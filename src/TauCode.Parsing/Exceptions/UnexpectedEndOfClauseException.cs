namespace TauCode.Parsing.Exceptions
{
    public class UnexpectedEndOfClauseException : ParseClauseFailedException
    {
        public UnexpectedEndOfClauseException(object[] partialParsingResults)
            : base("Unexpected end of clause.", partialParsingResults)
        {
        }
    }
}
