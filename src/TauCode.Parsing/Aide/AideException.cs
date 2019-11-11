using System;

namespace TauCode.Parsing.Aide
{
    [Serializable]
    public class AideException : Exception
    {
        public AideException(string message)
            : base(message)
        {
        }

        public AideException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
