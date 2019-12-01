using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Aide
{
    public interface IBuilder
    {
        INode Build(PseudoList defblocks);
    }
}
