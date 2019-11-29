using System;

namespace TauCode.Parsing.TinyLisp.Data
{
    public class Symbol : Atom
    {
        protected Symbol(string name, bool mustBeKeyword)
        {
            if (!TinyLispHelper.IsValidSymbolName(name, mustBeKeyword))
            {
                throw new NotImplementedException(); // todo
            }

            this.Name = name.ToUpperInvariant();
        }

        public Symbol(string name)
            : this(name, false)
        {
        }

        public string Name { get; }

        public override bool Equals(Element other)
        {
            if (other == null)
            {
                return false;
            }

            if (other.GetType() == this.GetType())
            {
                var otherSymbol = (Symbol)other;
                return this.Name.Equals(otherSymbol.Name);
            }

            return false;
        }

        public override int GetHashCode() => this.Name.GetHashCode();
    }
}
