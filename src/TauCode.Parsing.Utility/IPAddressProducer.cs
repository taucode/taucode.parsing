using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Utility
{
    public class IPAddressProducer : ITokenProducer
    {
        private static readonly HashSet<char> AcceptableChars;

        private static readonly int MinLength = "0.0.0.1".Length;
        private static readonly int MaxLength = "2001:0db8:85a3:0000:0000:8a2e:0370:7334".Length;

        private readonly Func<char, bool> _terminatingCharPredicate;

        public IPAddressProducer(Func<char, bool> terminatingCharPredicate)
        {
            _terminatingCharPredicate = 
                terminatingCharPredicate ??
                throw new ArgumentNullException(nameof(terminatingCharPredicate));
        }

        static IPAddressProducer()
        {
            var list = new List<char>();
            list.AddCharRange('a', 'f');
            list.AddCharRange('A', 'F');
            list.AddCharRange('0', '9');
            list.Add('.');
            list.Add(':');

            AcceptableChars = list.ToHashSet();
        }

        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = context.Length;

            var c = text[context.Index];

            var start = context.Index;
            var index = start;

            while (true)
            {
                if (index - start > MaxLength)
                {
                    return null;
                }

                if (index == length)
                {
                    break;
                }

                var isTerminator = _terminatingCharPredicate(c);
                if (isTerminator)
                {
                    break;
                }

                var isAcceptable = AcceptableChars.Contains(c);
                if (isAcceptable)
                {
                    index++;
                    continue;
                }

                return null; // not our char
            }

            var delta = index - start;
            if (delta < MinLength)
            {
                return null;
            }

            var span = text.AsSpan(start, delta);

            var parsed = IPAddress.TryParse(span, out var ipAddress);
            if (parsed)
            {
                var position = new Position(context.Line, start);
                return new IPAddressToken(ipAddress, position, delta);
            }

            return null;
        }
    }
}
