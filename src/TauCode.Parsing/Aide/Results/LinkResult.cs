namespace TauCode.Parsing.Aide.Results
{
    public class LinkResult : UnitResult
    {
        public LinkResult(string sourceNodeName)
            : base(sourceNodeName)
        {
            this.Arguments = new NameReferenceCollector();
        }

        public NameReferenceCollector Arguments { get; }
    }
}
