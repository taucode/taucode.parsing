namespace TauCode.Parsing.Aide.Parsing
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
