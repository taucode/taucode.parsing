namespace TauCode.Parsing
{
    public interface ITextClass
    {
        string TryConvertFrom(string text, ITextClass anotherTextClass);
    }
}
