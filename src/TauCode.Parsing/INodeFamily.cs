namespace TauCode.Parsing
{
    public interface INodeFamily
    {
        INode2[] GetNodes();
        void AddLink(string fromName, string toName);
    }
}
