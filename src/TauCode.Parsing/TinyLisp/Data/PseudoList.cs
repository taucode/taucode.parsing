using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TauCode.Parsing.TinyLisp.Data
{
    public class PseudoList : Atom, IReadOnlyList<Element>
    {
        #region Fields

        private readonly List<Element> _elements;

        #endregion

        #region Constructors

        public PseudoList()
        {
            _elements = new List<Element>();
        }

        public PseudoList(IEnumerable<Element> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            var list = elements.ToList();

            if (list.Any(x => x == null))
            {
                throw new ArgumentException($"'{nameof(elements)}' must not contain nulls.", nameof(elements));
            }

            _elements = list;
        }

        #endregion

        #region Overridden

        public override bool Equals(Element other) => ReferenceEquals(this, other);

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

        #endregion

        #region IReadOnlyList<Element> Members

        public IEnumerator<Element> GetEnumerator() => _elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

        public int Count => _elements.Count;

        public Element this[int index] => _elements[index];

        #endregion

        #region Public

        public void AddElement(Element element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            _elements.Add(element);
        }

        #endregion
    }
}
