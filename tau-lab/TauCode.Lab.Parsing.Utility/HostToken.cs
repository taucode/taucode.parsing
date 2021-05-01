using System;
using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Lab.Parsing.Utility
{
    public class HostToken : TokenBase
    {
        public HostToken(string host, UriHostNameType uriHostNameType, Position position, int consumedLength)
            : base(position, consumedLength)
        {
            this.Host = host ?? throw new ArgumentNullException(nameof(host));
            this.UriHostNameType = uriHostNameType;
        }

        public string Host { get; }

        public UriHostNameType UriHostNameType { get; }
    }
}
