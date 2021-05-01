namespace TauCode.Lab.Extensions.EmailValidation
{
    public enum EmailValidationError : byte
    {
        NoError = 0,

        EmailTooLong,
        LocalPartTooLong,
        UnexpectedEnd,
        SpacesOnlyAllowedAtEndOfLocalPart,
        LocalPartCannotEndWithPeriod,
        InvalidEscape,
        LocalPartCannotBeEmpty,

        DomainCannotBeEmpty,
        InvalidDomainName,

        InvalidIPv6Address,
    }
}
