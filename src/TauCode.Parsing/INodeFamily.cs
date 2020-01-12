namespace TauCode.Parsing
{
    public interface INodeFamily
    {
        string Name { get; }
        void RegisterNode(INode node);
        INode GetNode(string nodeName);
        INode[] GetNodes();
    }
}
