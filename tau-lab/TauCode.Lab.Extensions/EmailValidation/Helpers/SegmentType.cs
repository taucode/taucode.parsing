namespace TauCode.Lab.Extensions.EmailValidation.Helpers
{
    public enum SegmentType : byte
    {
        LocalPartWord = 1,
        LocalPartPeriod = 2,
        LocalPartSymbolSequence = 3,
        LocalPartQuotedString = 4,

        Comment = 5,

        At = 6, // '@' symbol

        DomainNamePart = 7,
        IPAddress = 8,

    }
}
