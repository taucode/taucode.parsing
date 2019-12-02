using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Building
{
    public interface IBuilder
    {
        INode Build(PseudoList defblocks);
    }
}
