namespace TauCode.Parsing.TinyLisp.Data
{
    public sealed class True : Symbol
    {
        public const string TrueSymbolName = "T";

        public static True Instance { get; }

        static True()
        {
            Instance = new True();
            RegisterSymbol(Instance);
        }

        private True()
            : base(TrueSymbolName)
        {
        }

        public override bool Equals(Element other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode() => this.GetType().GetHashCode();
    }
}
