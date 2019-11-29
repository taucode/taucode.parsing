using System.Collections.Generic;

namespace TauCode.Parsing.TinyLisp.Data
{
    public class PseudoList : Atom
    {
        private readonly List<Element> _items;

        public override bool Equals(Element other) => ReferenceEquals(this, other);
    }
}
