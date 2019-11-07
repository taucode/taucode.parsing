namespace TauCode.Parsing
{
    public interface IParsingContext
    {
        void AddResult(object result);
        T GetLastResult<T>();
        object[] ToArray();

        //void Add(string objectName, dynamic properties);
        //void Update(string objectName, dynamic properties);
        //dynamic Get(string objectName);
        //void Remove(string objectName);
    }
}
