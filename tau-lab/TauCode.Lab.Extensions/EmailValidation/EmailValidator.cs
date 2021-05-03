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
        private const int IPv4MaxLength = 15; // "101.101.101.101".Length

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

        #region .ctor

        public EmailValidator()
        {
            this.Settings = EmailValidationSettings.CreateDefault();
        }

        #endregion

        #region Public

        public EmailValidationResult Validate(in ReadOnlySpan<char> input, Func<char, bool> isTerminatingChar = null)
        {
            Span<Segment> segments = stackalloc Segment[EmailValidationExtensions.MaxLocalPartSegmentCount];
            var segmentCount = 0;

            var length = input.Length;

            byte index = 0;

            #region extract local part

            while (true)
            {
                if (index == length)
                {
                    return new EmailValidationResult(EmailValidationError.UnexpectedEnd, index);
                }

                if (index == EmailValidationExtensions.MaxEmailLength)
                {
                    return new EmailValidationResult(EmailValidationError.EmailTooLong, index);
                }

                var segment = this.ExtractLocalPartSegment(input, ref index, out var error);
                if (segment == null)
                {
                    return new EmailValidationResult(error, index);
                }

                var segmentValue = segment.Value;

                segments[segmentCount] = segmentValue;
                segmentCount++;

                if (segmentValue.Type == SegmentType.At)
                {
                    break;
                }
            }

            #endregion

            var localPartPlusAtSegmentCount = segmentCount;
            SegmentType? lastNonCommentSegmentType = null;

            #region extract domain

            // todo clean
            //var gotIpDomain = false;


            while (true)
            {
                if (index == length)
                {
                    if (segmentCount == localPartPlusAtSegmentCount)
                    {
                        return new EmailValidationResult(EmailValidationError.UnexpectedEnd, index);
                    }
                    else
                    {
                        // got to the end of the email, let's see what we're packing.
                        throw new NotImplementedException();
                    }
                }

                if (index == EmailValidationExtensions.MaxEmailLength)
                {
                    return new EmailValidationResult(EmailValidationError.EmailTooLong, index);
                }

                var segment = this.ExtractDomainSegment(
                    input,
                    lastNonCommentSegmentType,
                    ref index,
                    out var error);

                if (segment == null)
                {
                    return new EmailValidationResult(error, index);
                }

                segments[segmentCount] = segment.Value;
                segmentCount++;

                lastNonCommentSegmentType = segment.Value.Type;

            }

            #endregion


            throw new NotImplementedException();

            //Segment? lastLocalPartSegment = null;

            //#region extract local part

            //while (true)
            //{
            //    if (index == length)
            //    {
            //        throw new NotImplementedException();
            //    }

            //    var segment = this.ExtractLocalPartSegment(input, ref index, out var error);
            //    if (segment == null)
            //    {
            //        return new EmailValidationResult(error, index);
            //    }

            //    var segmentValue = segment.Value;

            //    if (segmentValue.Type == SegmentType.Period)
            //    {
            //        if (input[segmentValue.Start] == '.' && segmentCount == 0)
            //        {
            //            return new EmailValidationResult(EmailValidationError.LocalPartStartsWithPeriod, 0);
            //        }
            //    }
            //    else if (segmentValue.Type == SegmentType.LocalPartQuotedString)
            //    {
            //        if (this.QuotedStringSegmentIsEmpty(input, segmentValue))
            //        {
            //            return new EmailValidationResult(EmailValidationError.LocalPartStartsWithPeriod, 0);
            //        }
            //    }
            //    else if (segmentValue.Type == SegmentType.At)
            //    {
            //        if (segmentCount == 0)
            //        {
            //            // something like "@host.com"
            //            return new EmailValidationResult(EmailValidationError.EmptyLocalPart, 0);
            //        }

            //        // we're done with local part
            //        segments[segmentCount] = segmentValue;
            //        segmentCount++;
            //        break;
            //    }

            //    segments[segmentCount] = segmentValue;
            //    segmentCount++;

            //    lastLocalPartSegment = segmentValue;
            //}

            //#endregion

            //if (!lastLocalPartSegment.HasValue)
            //{
            //    // actually, shouldn't happen.
            //    return new EmailValidationResult(EmailValidationError.ValidationFailure, index);
            //}

            //var lastLocalPartSegmentValue = lastLocalPartSegment.Value;

            //if (lastLocalPartSegmentValue.Type == SegmentType.LocalPartWord)
            //{
            //    var idx = lastLocalPartSegmentValue.Start + lastLocalPartSegmentValue.Length - 1;
            //    var c = input[idx];
            //    if (c == '.')
            //    {
            //        return new EmailValidationResult(EmailValidationError.LocalPartEndsWithPeriod, (byte)idx);
            //    }
            //}

            //#region extract domain

            //var localPartSegmentCount = segmentCount;
            //var gotIpDomain = false;
            //Segment? lastDomainNamePartSegment = null;

            //while (true)
            //{
            //    if (index == length)
            //    {
            //        if (segmentCount == localPartSegmentCount)
            //        {
            //            return new EmailValidationResult(EmailValidationError.DomainCannotBeEmpty, index);
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }

            //    if (index > EmailValidationExtensions.MaxEmailLength)
            //    {
            //        throw new NotImplementedException();
            //    }

            //    var mustStartWithPeriod = false;

            //    if (lastDomainNamePartSegment.HasValue)
            //    {
            //        var c = input[lastDomainNamePartSegment.Value.Start + lastDomainNamePartSegment.Value.Length - 1];
            //        mustStartWithPeriod = c != '.';
            //    }

            //    // todo: TLD min length is 2.
            //    var segment = this.ExtractDomainSegment(
            //        input,
            //        gotIpDomain,
            //        mustStartWithPeriod,
            //        ref index,
            //        out var error);

            //    if (segment == null)
            //    {
            //        return new EmailValidationResult(error, index);
            //    }

            //    var segmentValue = segment.Value;

            //    if (segmentValue.Type == SegmentType.IPAddress)
            //    {
            //        gotIpDomain = true;
            //    }
            //    else if (segmentValue.Type == SegmentType.SubDomain)
            //    {
            //        lastDomainNamePartSegment = segmentValue;
            //    }

            //    segments[segmentCount] = segmentValue;
            //    segmentCount++;
            //}

            //#endregion

            //return new EmailValidationResult(EmailValidationError.NoError, null);

        }

        public EmailValidationSettings Settings { get; internal set; } // todo: public setter?

        #endregion

        #region Private

        private Segment? ExtractLocalPartSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var c = input[index];

            if (char.IsLetterOrDigit(c) || c == '_')
            {
                return this.ExtractLocalPartWordSegment(input, ref index, out error);
            }
            else if (c == '.')
            {
                byte start = index;
                index++;
                error = EmailValidationError.NoError;
                return new Segment(SegmentType.Period, start, 1);
            }
            else if (c == '@')
            {
                var start = index;
                index++;
                error = EmailValidationError.NoError;
                return new Segment(SegmentType.At, start, 1);
            }
            else if (this.Settings.EffectiveAllowedSymbols.Contains(c))
            {
                return this.ExtractLocalPartSymbolSequenceSegment(input, ref index, out error);
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

            error = EmailValidationError.UnexpectedCharacter;
            return null;
        }

        // todo clean
        private Segment? ExtractDomainSegment(
            in ReadOnlySpan<char> input,
            SegmentType? lastNonCommentSegmentType,
            //bool gotIpDomain,
            //bool mustStartWithPeriod,
            ref byte index,
            out EmailValidationError error)
        {
            var c = input[index];

            if (char.IsLetterOrDigit(c))
            {
                // we only want nothing or period before sub-domain segment
                if (
                    lastNonCommentSegmentType == null ||
                    lastNonCommentSegmentType == SegmentType.Period ||
                    false)
                {
                    return this.ExtractSubDomainSegment(input, ref index, out error);
                }
                else
                {
                    error = EmailValidationError.InvalidDomainName;
                    return null;
                }
            }
            else if (c == '.')
            {
                // we only want sub-domain before period segment
                if (lastNonCommentSegmentType == SegmentType.SubDomain)
                {
                    index++;
                    error = EmailValidationError.NoError;
                    return new Segment(SegmentType.Period, (byte)(index - 1), 1);
                }
                else
                {
                    error = EmailValidationError.InvalidDomainName;
                    return null;
                }
            }
            else if (c == '[')
            {
                // we only want nothing before ip address segment
                if (lastNonCommentSegmentType == null)
                {
                    if (index < input.Length - 1)
                    {
                        var nextChar = input[index + 1];
                        if (char.IsDigit(nextChar))
                        {
                            return this.ExtractIPv4Segment(input, ref index, out error);
                        }

                        if (nextChar == 'I') // start of 'IPv6:' signature
                        {
                            return this.ExtractIPv6Segment(input, ref index, out error);
                        }

                        index++;
                        error = EmailValidationError.UnexpectedCharacter;
                        return null;
                    }
                    else
                    {
                        index++;
                        error = EmailValidationError.UnexpectedEnd;
                        return null;
                    }
                }
                else
                {
                    error = EmailValidationError.UnexpectedCharacter;
                    return null;
                }

            }
            else if (c == '(')
            {
                return this.ExtractCommentSegment(input, ref index, out error);
            }
            else
            {
                error = EmailValidationError.UnexpectedCharacter; // todo: terminating char predicate here
                return null;
            }
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


        #endregion

        #region Common Extractors

        private Segment? ExtractCommentSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // input[start] is '('
            var length = input.Length;

            var depth = 1;

            while (true)
            {
                if (index == length)
                {
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                if (index == EmailValidationExtensions.MaxEmailLength)
                {
                    error = EmailValidationError.EmailTooLong; // todo: this will give wrong position. r&d this.
                    return null;
                }

                var c = input[index];
                if (c == ')')
                {
                    depth--;
                    if (depth == 0)
                    {
                        index++;
                        break;
                    }
                }
                else if (c == '(')
                {
                    depth++;
                }
                else if (c == '\\')
                {
                    index++; // skip '\\'
                    if (index == length)
                    {
                        error = EmailValidationError.UnexpectedEnd;
                        return null;
                    }

                    index++; // skip escaped symbol
                    if (index == length)
                    {
                        error = EmailValidationError.UnexpectedEnd;
                        return null;
                    }

                    continue;
                }

                index++;
            }

            var delta = index - start;
            error = EmailValidationError.NoError;
            return new Segment(SegmentType.Comment, start, (byte)delta);
        }

        #endregion

        #region Local Part Extractors

        private Segment? ExtractLocalPartSymbolSequenceSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // skip 0th symbol since we've got here
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

                if (this.Settings.EffectiveAllowedSymbols.Contains(c))
                {
                    index++;
                    continue;
                }

                // end of word.
                break;
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.LocalPartSymbolSequence, start, (byte)delta);
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
            return new Segment(SegmentType.LocalPartWord, start, (byte)delta);
        }

        private Segment? ExtractLocalPartQuotedStringSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // skip '"'
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

                if (c == '\\')
                {
                    index++; // skip '\\'

                    if (index == length)
                    {
                        error = EmailValidationError.UnclosedQuotedString;
                        return null;
                    }

                    index++; // skip escaped symbol

                    if (index == length)
                    {
                        error = EmailValidationError.UnclosedQuotedString;
                        return null;
                    }

                    continue;
                }

                index++;
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.LocalPartQuotedString, start, (byte)delta);
        }

        #endregion

        #region Domain Extractors

        private Segment? ExtractSubDomainSegment(
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
                    if (prevChar == '-')
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

                if (c == '-')
                {
                    if (prevChar == '.')
                    {
                        // '.' cannot be followed by '-'
                        error = EmailValidationError.InvalidDomainName;
                        return null;
                    }

                    prevChar = c;
                    index++;
                    continue;
                }

                if (c == '(')
                {
                    // got start of comment
                    break;
                }

                error = EmailValidationError.InvalidDomainName;
                return null;
            }

            error = EmailValidationError.NoError;
            var delta = index - start;
            return new Segment(SegmentType.SubDomain, start, (byte)delta);
        }

        private Segment? ExtractIPv6Segment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // initial char is ok since we've got here
            var length = (byte)input.Length;

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
                error = EmailValidationError.InvalidIPv6Address;
                return null;
            }

            ReadOnlySpan<char> prefixSpan = prefix;
            if (input.Slice(index, prefixLength).Equals(prefixSpan, StringComparison.Ordinal))
            {
                // good.
            }
            else
            {
                error = EmailValidationError.InvalidIPv6Address;
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
                    return new Segment(SegmentType.IPAddress, start, (byte)(index - start));
                }
            }

            index = addressStart;
            error = EmailValidationError.InvalidIPv6Address;
            return null;
        }

        private Segment? ExtractIPv4Segment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            index++; // skip '['
            var length = (byte)input.Length;

            var remaining = length - index;

            const int minRemainingLength = 8; // "1.1.1.1]".Length;

            if (remaining < minRemainingLength)
            {
                index = length;
                error = EmailValidationError.InvalidIPv4Address;
                return null;
            }

            // todo clean file
            //ReadOnlySpan<char> prefixSpan = prefix;
            //if (input.Slice(index, prefixLength).Equals(prefixSpan, StringComparison.Ordinal))
            //{
            //    // good.
            //}
            //else
            //{
            //    error = EmailValidationError.InvalidIPv6Prefix;
            //    return null;
            //}

            //index += prefixLength;
            var addressStart = index;

            while (true)
            {
                if (index == input.Length)
                {
                    error = EmailValidationError.UnexpectedEnd;
                    return null;
                }

                var currentAddressLength = index - addressStart;
                if (currentAddressLength > IPv4MaxLength)
                {
                    index = addressStart;
                    error = EmailValidationError.InvalidIPv4Address;
                    return null;
                }

                var c = input[index];
                if (char.IsDigit(c) || c == '.')
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
                error = EmailValidationError.InvalidIPv4Address;
                return null;
            }

            var delta = index - addressStart - 1; // '-1' because not include ']'
            var span = input.Slice(addressStart, delta);
            var parsed = IPAddress.TryParse(span, out var address);
            if (parsed)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    error = EmailValidationError.NoError;
                    return new Segment(SegmentType.IPAddress, start, (byte)(index - start));
                }
            }

            index = addressStart;
            error = EmailValidationError.InvalidIPv4Address;
            return null;
        }

        #endregion
    }
}
