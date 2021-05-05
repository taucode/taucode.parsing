namespace TauCode.Lab.Extensions.EmailValidation.Helpers
{
    public enum SegmentType : byte
    {
        Period = 1,
        Comment,

        LocalPartSpace,
        LocalPartFoldingWhiteSpace,
        LocalPartWord,
        LocalPartQuotedString,

        At, // '@' symbol

        SubDomain,
        IPAddress,
    }
}
