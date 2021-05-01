using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using TauCode.Extensions;
using TauCode.Lab.Extensions.EmailValidation.Helpers;

namespace TauCode.Lab.Extensions.EmailValidation
{
    public class EmailValidator
    {
        #region Constants

        private const int IPv6MaxLength = 55; // "eeee:eeee:eeee:eeee:eeee:eeee:eeee:eeee:111.111.111.111".Length;
        private static readonly HashSet<char> IPv6AcceptableChars;

        #endregion

        #region Static .ctor

        static EmailValidator()
        {
            var list = new List<char>();
            list.AddCharRange('a', 'f');
            list.AddCharRange('A', 'F');
            list.AddCharRange('0', '9');
            list.Add('.');
            list.Add(':');

            IPv6AcceptableChars = list.ToHashSet();
        }

        #endregion

        public EmailValidator()
        {
            this.Settings = EmailValidationSettings.CreateDefault();
        }

        public EmailValidationResult Validate(in ReadOnlySpan<char> input)
        {
            Span<Segment> segments = stackalloc Segment[EmailValidationExtensions.MaxLocalPartSegmentCount];
            var segmentCount = 0;

            var length = input.Length;

            if (length > EmailValidationExtensions.MaxEmailLength)
            {
                return new EmailValidationResult(EmailValidationError.EmailTooLong, null);
            }

            byte index = 0;

            #region extract local part

            while (true)
            {
                if (index == length)
                {
                    throw new NotImplementedException();
                }

                var segment = this.ExtractLocalPartSegment(input, ref index, out var error);
                if (segment == null)
                {
                    return new EmailValidationResult(error, index);
                }

                var segmentValue = segment.Value;

                if (segmentValue.Type == SegmentType.LocalPartPart)
                {
                    if (input[segmentValue.Start] == '.' && segmentCount == 0)
                    {
                        return new EmailValidationResult(EmailValidationError.LocalPartStartsWithPeriod, 0);
                    }
                }
                else if (segmentValue.Type == SegmentType.LocalPartQuotedString)
                {
                    if (this.QuotedStringSegmentIsEmpty(input, segmentValue))
                    {
                        return new EmailValidationResult(EmailValidationError.LocalPartStartsWithPeriod, 0);
                    }
                }
                else if (segmentValue.Type == SegmentType.At)
                {
                    if (segmentCount == 0)
                    {
                        // something like "@host.com"
                        return new EmailValidationResult(EmailValidationError.EmptyLocalPart, 0);
                    }

                    // we're done with local part
                    segments[segmentCount] = segmentValue;
                    segmentCount++;
                    break;
                }

                segments[segmentCount] = segmentValue;
                segmentCount++;
            }

            #endregion

            #region extract domain

            var localPartSegmentCount = segmentCount;
            var gotIpDomain = false;
            Segment? lastDomainNamePartSegment = null;

            while (true)
            {
                if (index == length)
                {
                    if (segmentCount == localPartSegmentCount)
                    {
                        return new EmailValidationResult(EmailValidationError.DomainCannotBeEmpty, index);
                    }
                    else
                    {
                        break;
                    }
                }

                if (index > EmailValidationExtensions.MaxEmailLength)
                {
                    throw new NotImplementedException();
                }

                bool mustStartWithPeriod = false;

                if (lastDomainNamePartSegment.HasValue)
                {
                    var c = input[lastDomainNamePartSegment.Value.Start + lastDomainNamePartSegment.Value.Length];
                    mustStartWithPeriod = c != '.';
                }

                // todo: TLD min length is 2.
                var segment = this.ExtractDomainSegment(
                    input,
                    gotIpDomain,
                    mustStartWithPeriod,
                    ref index,
                    out var error);

                if (segment == null)
                {
                    return new EmailValidationResult(error, index);
                }

                var segmentValue = segment.Value;

                if (segmentValue.Type == SegmentType.IPAddress)
                {
                    gotIpDomain = true;
                }
                else if (segmentValue.Type == SegmentType.DomainNamePart)
                {
                    lastDomainNamePartSegment = segmentValue;
                }

                segments[segmentCount] = segmentValue;
                segmentCount++;
            }

            #endregion

            return new EmailValidationResult(EmailValidationError.NoError, null);

        }

        private bool QuotedStringSegmentIsEmpty(in ReadOnlySpan<char> input, in Segment segment)
        {
            if (segment.Length == 2)
            {
                return true;
            }

            for (var i = 0; i < segment.Length; i++)
            {
                var index = segment.Start + i;
                var c = input[index];

                if (!char.IsWhiteSpace(c))
                {
                    return false;
                }
            }

            return true;
        }

        #region Common Extractors

        private Segment? ExtractCommentSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // input[start] is '('
            var length = input.Length;

            while (true)
            {
                if (index == length)
                {
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                var c = input[index];
                if (c == ')')
                {
                    index++;
                    break;
                }

                index++;
            }

            var delta = index - start;
            error = EmailValidationError.NoError;
            return new Segment(SegmentType.Comment, start, (byte)delta);
        }

        #endregion

        #region Local Part Extractors

        private Segment? ExtractLocalPartSpecialCharacterSequenceSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // input[start] is a proper special character since we've got here
            var length = input.Length;

            while (true)
            {
                if (index - start > EmailValidationExtensions.MaxLocalPartLength)
                {
                    error = EmailValidationError.LocalPartTooLong;
                    return null;
                }

                if (index == length)
                {
                    // end of sequence.
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                var c = input[index];

                if (this.Settings.EffectiveAllowedSpecialCharacters.Contains(c))
                {
                    index++;
                    continue;
                }

                // end of word.
                break;
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.LocalPartSpecialCharacterSequence, start, (byte) delta);
        }

        private Segment? ExtractLocalPartWordSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // input[start] is a proper char since we've got here
            var length = input.Length;

            while (true)
            {
                if (index - start > EmailValidationExtensions.MaxLocalPartLength)
                {
                    error = EmailValidationError.LocalPartTooLong;
                    return null;
                }

                if (index == length)
                {
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                var c = input[index];

                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    index++;
                    continue;
                }

                // end of word.
                break;
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.LocalPartPart, start, (byte) delta);
        }

        private Segment? ExtractLocalPartSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var c = input[index];

            if (char.IsLetterOrDigit(c) || c == '_' || c == '.')
            {
                return this.ExtractLocalPartWordSegment(input, ref index, out error);
            }
            else if (c == '@')
            {
                var start = index;
                index++;
                error = EmailValidationError.NoError;
                return new Segment(SegmentType.At, start, 1);
            }
            else if (this.Settings.EffectiveAllowedSpecialCharacters.Contains(c))
            {
                return this.ExtractLocalPartSpecialCharacterSequenceSegment(input, ref index, out error);
            }
            else if (char.IsWhiteSpace(c))
            {
                error = EmailValidationError.UnexpectedSpace;
                return null;
            }
            else if (c == '"')
            {
                return this.ExtractLocalPartQuotedStringSegment(input, ref index, out error);
            }
            else if (c == '(')
            {
                return this.ExtractCommentSegment(input, ref index, out error);
            }
            else
            {
                throw new NotImplementedException();
            }


            throw new NotImplementedException();
        }

        private Segment? ExtractLocalPartQuotedStringSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // input[start] is a proper special character since we've got here
            var length = input.Length;

            while (true)
            {
                if (index - start > EmailValidationExtensions.MaxLocalPartLength)
                {
                    error = EmailValidationError.LocalPartTooLong;
                    return null;
                }

                if (index == length)
                {
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                var c = input[index];
                if (c == '"')
                {
                    index++;
                    break;
                }

                index++;
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.LocalPartQuotedString, start, (byte) delta);
        }

        #endregion

        #region Domain Extractors

        private Segment? ExtractDomainNamePartSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            var prevChar = input[start];
            index++; // initial char is ok since we've got here
            var length = input.Length;

            while (true)
            {
                if (index == length)
                {
                    if (prevChar == '.' || prevChar == '-')
                    {
                        error = EmailValidationError.InvalidDomainName;
                        return null;
                    }

                    break;
                }

                var c = input[index];

                if (char.IsLetterOrDigit(c))
                {
                    prevChar = c;
                    index++;
                    continue;
                }

                if (c == '.')
                {
                    if (prevChar == '.' || prevChar == '-')
                    {
                        error = EmailValidationError.InvalidDomainName;
                        return null;
                    }

                    prevChar = c;
                    index++;
                    continue;
                }

                throw new NotImplementedException();
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.DomainNamePart, start, (byte) delta);
        }

        private Segment? ExtractDomainSegment(
            in ReadOnlySpan<char> input,
            bool gotIpDomain,
            bool mustStartWithPeriod,
            ref byte index,
            out EmailValidationError error)
        {
            var c = input[index];

            if (char.IsLetterOrDigit(c))
            {
                if (mustStartWithPeriod)
                {
                    error = EmailValidationError.InvalidDomainName;
                    return null;
                }

                if (gotIpDomain)
                {
                    error = EmailValidationError.InvalidDomainName;
                    return null;
                }
                else
                {
                    return this.ExtractDomainNamePartSegment(input, ref index, out error);
                }
            }
            else if (c == '.')
            {
                if (mustStartWithPeriod)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    error = EmailValidationError.InvalidDomainName;
                    return null;
                }
            }
            else if (c == '[')
            {
                if (gotIpDomain)
                {
                    error = EmailValidationError.UnexpectedCharacter;
                    return null;
                }

                if (index < input.Length - 1)
                {
                    var nextChar = input[index + 1];
                    if (char.IsDigit(nextChar))
                    {
                        return this.ExtractIPv4Segment(input, ref index, out error);
                    }
                    else
                    {
                        return this.ExtractIPv6Segment(input, ref index, out error);
                    }
                }
            }
            else
            {
                error = EmailValidationError.UnexpectedCharacter;
                return null;
            }

            throw new NotImplementedException();
        }

        private Segment? ExtractIPv6Segment(in ReadOnlySpan<char> input, ref byte index, out EmailValidationError error)
        {
            var start = index;
            index++; // initial char is ok since we've got here
            var length = (byte) input.Length;

            const string prefix = "IPv6:";
            const int prefixLength = 5; // "IPv6:".Length
            const int minRemainingLength =
                prefixLength +
                2 + /* :: */
                1; /* ] */

            var remaining = length - index;

            if (remaining < minRemainingLength)
            {
                index = length;
                error = EmailValidationError.UnexpectedEnd;
                return null;
            }

            ReadOnlySpan<char> prefixSpan = prefix;
            if (input.Slice(index, prefixLength).Equals(prefixSpan, StringComparison.Ordinal))
            {
                // good.
            }
            else
            {
                error = EmailValidationError.InvalidIPv6Prefix;
                return null;
            }

            index += prefixLength;
            var addressStart = index;

            while (true)
            {
                if (index == input.Length)
                {
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                var currentAddressLength = index - addressStart;
                if (currentAddressLength > IPv6MaxLength)
                {
                    index = addressStart;
                    error = EmailValidationError.InvalidIPv6Address;
                    return null;
                }

                var c = input[index];
                if (IPv6AcceptableChars.Contains(c))
                {
                    index++;
                    continue;
                }

                if (c == ']')
                {
                    index++;
                    break;
                }

                index = addressStart;
                error = EmailValidationError.InvalidIPv6Address;
                return null;
            }

            var delta = index - addressStart - 1; // '-1' because not include ']'
            var span = input.Slice(addressStart, delta);
            var parsed = IPAddress.TryParse(span, out var address);
            if (parsed)
            {
                if (address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    error = EmailValidationError.NoError;
                    return new Segment(SegmentType.IPAddress, start, (byte) (index - start));
                }
            }

            index = addressStart;
            error = EmailValidationError.InvalidIPv6Address;
            return null;
        }

        private Segment? ExtractIPv4Segment(in ReadOnlySpan<char> input, ref byte index, out EmailValidationError error)
        {
            throw new NotImplementedException();
        }

        #endregion

        public EmailValidationSettings Settings { get; internal set; }
    }
}
