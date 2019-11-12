using TauCode.Parsing.Aide.Results;
using TauCode.Parsing.Units;

namespace TauCode.Parsing.Aide
{
    public interface IAideBuilder
    {
        IBlock BuildMainBlock(IAideResult[] structure, IBuildEnvironment buildEnvironment);
    }
}
