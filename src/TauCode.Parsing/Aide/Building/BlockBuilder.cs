using TauCode.Parsing.Aide.Results;

namespace TauCode.Parsing.Aide.Building
{
    public class BlockBuilder
    {
        public BlockBuilder(Boss boss, BlockDefinitionResult source)
        {
            this.Boss = boss;
        }

        public Boss Boss { get; }
        public BlockDefinitionResult Source { get; set; }
    }
}
