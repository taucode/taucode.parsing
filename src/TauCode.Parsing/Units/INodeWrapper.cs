namespace TauCode.Parsing.Units
{
    public interface INodeWrapper : IUnit
    {
        INode InternalNode { get; set; }
        void AddDeferredLink(IUnit unit);
    }
}
