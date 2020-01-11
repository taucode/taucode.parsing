using System;

namespace TauCode.Parsing.Tests.Parsing.Cli.Exceptions
{
    [Serializable]
    public class CliException : Exception
    {
        public CliException()
        {
        }

        public CliException(string message) : base(message)
        {
        }

        public CliException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
