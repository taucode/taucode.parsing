namespace TauCode.Parsing.Aide.Results
{
    public class CloneBlockResult : IAideResult
    {
        public CloneBlockResult()
        {
            this.Arguments = new NameReferenceCollector();
        }

        public NameReferenceCollector Arguments { get; }
    }
}
