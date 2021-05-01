using System.Net;
using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Lab.Parsing.Utility
{
    public class IPAddressToken : TokenBase

    {
        public IPAddressToken(
            IPAddress address,
            Position position,
            int consumedLength)
            : base(position, consumedLength)
        {
            this.Address = address;
        }

        public IPAddress Address { get; }
    }
}
