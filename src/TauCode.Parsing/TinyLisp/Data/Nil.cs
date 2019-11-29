namespace TauCode.Parsing.TinyLisp.Data
{
    public sealed class Nil : Atom
    {
        public static Nil Instance { get; } = new Nil();

        private Nil()
        {   
        }

        public override bool Equals(Element other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode() => this.GetType().GetHashCode();
    }
}
