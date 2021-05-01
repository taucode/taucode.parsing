namespace TauCode.Lab.Extensions.EmailValidation
{
    public enum EmailValidationError : byte
    {
        NoError = 0,

        EmailTooLong,
        LocalPartTooLong,
        UnexpectedEnd,
        SpacesOnlyAllowedAtEndOfLocalPart,
        LocalPartCannotStartWithPeriod,
        LocalPartCannotEndWithPeriod,
        InvalidEscape,
        LocalPartCannotBeEmpty,
        UnclosedQuotedString,

        DomainCannotBeEmpty,
        InvalidDomainName,

        InvalidIPv6Address,
    }
}
