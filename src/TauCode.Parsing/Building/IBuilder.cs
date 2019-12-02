using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public interface IBuilder
    {
        INode Build(INodeFactory nodeFactory, PseudoList defblocks);
    }
}
