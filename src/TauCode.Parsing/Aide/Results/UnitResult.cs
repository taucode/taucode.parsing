namespace TauCode.Parsing.Aide.Results
{
    public abstract class UnitResult : IAideResult
    {
        protected UnitResult(string sourceNodeName)
        {
            this.SourceNodeName = sourceNodeName;
        }

        public string SourceNodeName { get; }
    }
}
