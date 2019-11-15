namespace TauCode.Parsing
{
    public interface INodeFamily
    {
        string Name { get; }
        INode2 GetNode(string linkAddress);
        INode2[] GetNodes();
        void AddLink(string fromName, string toName);
    }
}
