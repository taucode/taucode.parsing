using TauCode.Parsing.Building;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Old.Building
{
    public abstract class OldNodeFactoryBase : INodeFactory
    {
        protected OldNodeFactoryBase(string nodeFamilyName)
        {
            this.NodeFamily = new NodeFamily(nodeFamilyName);
        }

        public INodeFamily NodeFamily { get; }
        public abstract INode CreateNode(PseudoList item);
    }
}
