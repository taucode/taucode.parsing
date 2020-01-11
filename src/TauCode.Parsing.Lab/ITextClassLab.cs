namespace TauCode.Parsing.Lab
{
    public interface ITextClassLab
    {
        string Tag { get; }
        string TryConvertFrom(string text, ITextClassLab anotherClass);
    }
}
