using System;

namespace TauCode.Parsing.Exceptions
{
    /// <summary>
    /// Base class for all parsing-related exceptions.
    /// It is marked as non-abstract since using abstract exceptions is a malpractice in .NET.
    /// However, its constructors are marked protected to avoid instantiation of this ("semantically" abstract) class directly.
    /// </summary>
    [Serializable]
    public class ParsingExceptionBase : Exception
    {
        protected ParsingExceptionBase(string message)
            : base(message)
        {
        }

        public ParsingExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
