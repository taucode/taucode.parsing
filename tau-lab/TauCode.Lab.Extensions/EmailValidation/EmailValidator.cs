using System;
using TauCode.Lab.Extensions.EmailValidation.Helpers;

namespace TauCode.Lab.Extensions.EmailValidation
{
    public class EmailValidator
    {
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
                    throw new NotImplementedException();
                }

                var segmentValue = segment.Value;
                if (segmentValue.Type == SegmentType.At)
                {
                    if (segmentCount == 0)
                    {
                        // something like "@host.com"
                        return new EmailValidationResult(EmailValidationError.LocalPartCannotBeEmpty, 0);
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

                // todo: TLD min length is 2.
                var segment = this.ExtractDomainSegment(input, gotIpDomain, ref index, out var error);
                if (segment == null)
                {
                    throw new NotImplementedException();
                }

                var segmentValue = segment.Value;

                if (segmentValue.Type == SegmentType.IPAddress)
                {
                    gotIpDomain = true;
                }

                segments[segmentCount] = segmentValue;
                segmentCount++;
            }

            #endregion

            return new EmailValidationResult(EmailValidationError.NoError, null);

        }

        private Segment? ExtractDomainSegment(
            in ReadOnlySpan<char> input,
            bool gotIpDomain,
            ref byte index,
            out EmailValidationError error)
        {
            var c = input[index];

            if (char.IsLetterOrDigit(c))
            {
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
            else
            {
                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        private Segment? ExtractDomainNamePartSegment(
            in ReadOnlySpan<char> input,
            ref byte index,
            out EmailValidationError error)
        {
            var start = index;
            var prevChar = input[start];
            index++;
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

        private Segment? ExtractLocalPartSegment(in ReadOnlySpan<char> input, ref byte index, out EmailValidationError error)
        {
            var c = input[index];

            if (char.IsLetterOrDigit(c) || c == '_')
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
            else
            {
                throw new NotImplementedException();
            }


            throw new NotImplementedException();
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
                    // end of word.
                    break;
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
        
        public EmailValidationSettings Settings { get; internal set; }
    }
}
