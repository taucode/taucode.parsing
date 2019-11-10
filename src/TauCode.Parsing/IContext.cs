namespace TauCode.Parsing
{
    public interface IContext
    {
        void AddResult(object result);
        T GetLastResult<T>();
        int ResultCount { get; }
        object[] ToArray();
        int Version { get; }
        void Modify();
    }
}
