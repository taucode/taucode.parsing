namespace TauCode.Parsing.TinyLisp.Data
{
    public sealed class True : Atom
    {
        public static True Instance { get; } = new True();

        private True()
        {   
        }

        public override bool Equals(Element other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode() => this.GetType().GetHashCode();
    }
}
