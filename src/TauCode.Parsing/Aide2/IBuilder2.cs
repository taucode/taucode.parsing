using TauCode.Parsing.TinyLisp.Data;

namespace TauCode.Parsing.Aide2
{
    public interface IBuilder2
    {
        INode Build(PseudoList defblocks);
    }
}
