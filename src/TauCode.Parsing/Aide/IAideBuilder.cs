using TauCode.Parsing.Units;

namespace TauCode.Parsing.Aide
{
    public interface IAideBuilder
    {
        IBlock BuildBlock(IBuildEnvironment buildEnvironment);
    }
}
