namespace TauCode.Parsing.Aide.Results
{
    public class OptionalResult : UnitResult, IContentOwner
    {
        public OptionalResult(string tag)
            : base(tag)
        {
            this.OptionalContent = new Content(this);
        }

        public Content OptionalContent { get; }
    }
}
