using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public abstract class NodeFactoryBase : INodeFactory
    {
        protected NodeFactoryBase(string nodeFamilyName)
        {
            this.NodeFamily = new NodeFamily(nodeFamilyName);
        }

        public INodeFamily NodeFamily { get; }
        public abstract INode CreateNode(PseudoList item);
    }
}
