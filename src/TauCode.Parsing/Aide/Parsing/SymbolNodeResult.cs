using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Aide.Parsing
{
    public class SymbolNodeResult : NodeResult
    {
        public SymbolNodeResult(SymbolValue value)
        {
            this.Value = value;
        }

        public SymbolValue Value { get; }
    }
}
