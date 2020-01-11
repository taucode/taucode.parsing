using System;
using TauCode.Parsing.Building;
using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Lab.Building
{
    public class NodeFactoryBaseLab : INodeFactory
    {
        protected NodeFactoryBaseLab(string nodeFamilyName)
        {
            this.NodeFamily = new NodeFamily(nodeFamilyName);
        }

        public INodeFamily NodeFamily { get; }

        public virtual INode CreateNode(PseudoList item)
        {
            throw new NotImplementedException();
        }
    }
}
