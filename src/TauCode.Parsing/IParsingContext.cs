namespace TauCode.Parsing
{
    public interface IParsingContext
    {
        void Add(string objectName, dynamic properties);
        void Update(string objectName, dynamic properties);
        dynamic Get(string objectName);
        void Remove(string objectName);
    }
}
