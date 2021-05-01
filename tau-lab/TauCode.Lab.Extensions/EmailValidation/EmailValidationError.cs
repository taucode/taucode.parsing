namespace TauCode.Lab.Extensions.EmailValidation
{
    public enum EmailValidationError : byte
    {
        NoError = 0,

        EmailTooLong,
        LocalPartTooLong,
        UnexpectedEnd,
        UnexpectedSpace,
        UnexpectedCharacter,
        LocalPartStartsWithPeriod,
        LocalPartEndsWithPeriod,
        InvalidEscape,
        EmptyLocalPart,
        UnclosedQuotedString,

        DomainCannotBeEmpty,
        InvalidDomainName,

        InvalidIPv6Prefix,
        InvalidIPv6Address,
    }
}
