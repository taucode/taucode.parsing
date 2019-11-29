using System;

namespace TauCode.Parsing.TinyLisp.Data
{
    public sealed class StringAtom : Atom
    {
        public StringAtom(string value)
        {
            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Value { get; }

        public override bool Equals(Element other)
        {
            if (other is StringAtom otherStringAtom)
            {
                return this.Value.Equals(otherStringAtom.Value, StringComparison.InvariantCulture);
            }

            return false;
        }

        public override int GetHashCode() => this.Value.GetHashCode();
    }
}
