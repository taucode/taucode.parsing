namespace TauCode.Lab.Extensions.EmailValidation
{
    public enum EmailValidationError : byte
    {
        NoError = 0,

        UnexpectedEnd,

        EmailTooLong,
        LocalPartTooLong,
        UnexpectedCharacter,
        EmptyLocalPart,

        UnclosedQuotedString,
        EmptyString,
        NullCharacterMustBeEscaped,
        QuotedStringContainsCr,
        QuotedStringContainsFoldingWhiteSpace,

        InvalidDomainName,
        InvalidIPv4Address,
        InvalidIPv6Address,
    }
}
