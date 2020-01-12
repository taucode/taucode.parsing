using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TauCode.Parsing.Exceptions
{
    [Serializable]
    public class AlphaException : Exception
    {
        public AlphaException()
        {
        }

        public AlphaException(string message) : base(message)
        {
        }

        public AlphaException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
