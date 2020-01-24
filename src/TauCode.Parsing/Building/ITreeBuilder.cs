using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public interface ITreeBuilder
    {
        INode Build(INodeFactory nodeFactory, PseudoList defblocks);
    }
}
