namespace TauCode.Parsing.Aide.Results
{
    public class BlockDefinitionResult : IContentOwner
    {
        public BlockDefinitionResult()
        {
            this.Content = new Content(this);
            this.Arguments = new NameReferenceCollector();
        }

        public NameReferenceCollector Arguments { get; }

        public Content Content { get; }
    }
}
