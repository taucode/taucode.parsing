namespace TauCode.Parsing
{
    public interface INodeFamily
    {
        string Name { get; }
        INode GetNode(string linkAddress);
        INode[] GetNodes();
        void AddLink(string fromName, string toName);
    }
}
