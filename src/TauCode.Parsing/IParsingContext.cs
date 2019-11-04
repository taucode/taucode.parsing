namespace TauCode.Parsing
{
    public interface IParsingContext
    {
        void Push(string objectName, dynamic properties);
    }
}
