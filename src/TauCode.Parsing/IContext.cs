namespace TauCode.Parsing
{
    public interface IContext
    {
        void AddResult(object result);
        T GetLastResult<T>();
        object[] ToArray();
    }
}
