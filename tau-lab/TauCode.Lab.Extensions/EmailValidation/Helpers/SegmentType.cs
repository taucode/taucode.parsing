namespace TauCode.Lab.Extensions.EmailValidation.Helpers
{
    public enum SegmentType : byte
    {
        Period = 1,
        Comment,

        LocalPartWord,
        LocalPartSymbolSequence,
        LocalPartQuotedString,

        At, // '@' symbol

        SubDomain,
        IPAddress,
    }
}
