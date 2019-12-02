using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public interface INodeFactory
    {
        INodeFamily NodeFamily { get; }
        INode CreateNode(PseudoList item);
    }
}
