namespace TauCode.Parsing
{
    public interface INodeFamily
    {
        string Name { get; }
        INode GetNode(string nodeName);
        INode[] GetNodes();
    }
}
