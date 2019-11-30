namespace TauCode.Parsing.TinyLisp.Data
{
    public sealed class Nil : Symbol
    {
        public const string NilSymbolName = "NIL";

        public static Nil Instance { get; }

        static Nil()
        {
            Instance = new Nil();
            RegisterSymbol(Instance);
        }

        private Nil()
            : base(NilSymbolName)
        {
        }

        public override bool Equals(Element other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode() => this.GetType().GetHashCode();
    }
}
