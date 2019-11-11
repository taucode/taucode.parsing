namespace TauCode.Parsing.Aide.Results
{
    public abstract class UnitResult
    {
        protected UnitResult(string tag)
        {
            this.Tag = tag;
        }

        public string Tag { get; }
    }
}
