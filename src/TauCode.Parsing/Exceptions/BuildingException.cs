using System;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class BuildingException : Exception
    {
        public BuildingException(string message)
            : base(message)
        {
        }

        public BuildingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
