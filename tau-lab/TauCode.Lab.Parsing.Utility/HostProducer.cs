using System;
using TauCode.Extensions;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;

namespace TauCode.Lab.Parsing.Utility
{
    public class HostProducer : ITokenProducer
    {
        private const int MinLength = 1;
        private const int MaxLength = 255;

        private readonly Func<char, bool> _terminatingCharPredicate;

        public HostProducer(Func<char, bool> terminatingCharPredicate)
        {
            _terminatingCharPredicate =
                terminatingCharPredicate ??
                throw new ArgumentNullException(nameof(terminatingCharPredicate));
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

                var isAcceptable =
                    c == '.' ||
                    c == '-' ||
                    char.IsLetterOrDigit(c);

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

            var possibleHost = text.Substring(start, length);
            var res = Uri.CheckHostName(possibleHost);

            if (res.IsIn(
                UriHostNameType.Dns,
                UriHostNameType.IPv4,
                UriHostNameType.IPv6))
            {
                var position = new Position(context.Line, start);
                return new HostToken(possibleHost, res, position, delta);
            }

            return null;
        }
    }
}
