namespace TauCode.Parsing.Aide.Results
{
    public class CloneBlockResult
    {
        public CloneBlockResult()
        {
            this.Arguments = new NameReferenceCollector();
        }

        public NameReferenceCollector Arguments { get; }
    }
}
