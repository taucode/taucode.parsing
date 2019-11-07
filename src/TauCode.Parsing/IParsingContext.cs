namespace TauCode.Parsing
{
    public interface IParsingContext
    {
        void AddResult(object result);
        T GetLastResult<T>();
        object[] ToArray();
    }
}
