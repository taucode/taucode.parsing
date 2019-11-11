namespace TauCode.Parsing.Aide.Results
{
    public abstract class UnitResult
    {
        protected UnitResult(string sourceNodeName)
        {
            this.SourceNodeName = sourceNodeName;
        }

        public string SourceNodeName { get; }
    }
}
