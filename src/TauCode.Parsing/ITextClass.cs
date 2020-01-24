namespace TauCode.Parsing
{
    public interface ITextClass
    {
        string Tag { get; }
        string TryConvertFrom(string text, ITextClass anotherClass);
    }
}
