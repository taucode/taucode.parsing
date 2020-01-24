using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class ParseClauseFailedException : ParsingExceptionBase
    {
        protected ParseClauseFailedException(string message, object[] partialParsingResults)
            : base(message)
        {
            this.PartialParsingResults = partialParsingResults;
        }

        protected ParseClauseFailedException(string message, Exception innerException, object[] partialParsingResults)
            : base(message, innerException)
        {
            this.PartialParsingResults = partialParsingResults;
        }

        public object[] PartialParsingResults { get; }
    }
}
