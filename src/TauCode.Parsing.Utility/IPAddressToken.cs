using System.Net;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Utility
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
