using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.TinyLisp.Data
{
    public class PseudoList : Atom, IReadOnlyList<Element>
    {
        private readonly List<Element> _elements;

        public PseudoList()
        {
            _elements = new List<Element>();
        }

        public IReadOnlyList<Element> Elements => _elements;

        public void AddElement(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            _elements.Add(element);
        }

        public override bool Equals(Element other) => ReferenceEquals(this, other);
        public IEnumerator<Element> GetEnumerator() => _elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");

            for (var i = 0; i < _elements.Count; i++)
            {
                var element = _elements[i];
                sb.Append(element);

                if (i < _elements.Count - 1)
                {
                    sb.Append(" ");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }

        public int Count => _elements.Count;

        public Element this[int index] => _elements[index];
    }
}
