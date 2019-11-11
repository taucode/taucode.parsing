namespace TauCode.Parsing.Aide.Results
{
    public class SyntaxElementResult : UnitResult
    {
        public SyntaxElementResult(SyntaxElement syntaxElement, string sourceNodeName)
            : base(sourceNodeName)
        {
            this.SyntaxElement = syntaxElement;
            this.Arguments = new NameReferenceCollector();
        }

        public SyntaxElement SyntaxElement { get; }

        public NameReferenceCollector Arguments { get; }
    }
}
