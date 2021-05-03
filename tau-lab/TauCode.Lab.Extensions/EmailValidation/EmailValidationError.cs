namespace TauCode.Lab.Extensions.EmailValidation
{
    public enum EmailValidationError : byte
    {
        NoError = 0,

        UnexpectedEnd,

        EmailTooLong,
        LocalPartTooLong,
        UnexpectedSpace,
        UnexpectedCharacter,
        LocalPartStartsWithPeriod,
        LocalPartEndsWithPeriod,
        InvalidEscape,
        EmptyLocalPart,
        UnclosedQuotedString,

        ValidationFailure,

        InvalidDomainName,
        InvalidIPv4Address,
        InvalidIPv6Address,
    }
}
