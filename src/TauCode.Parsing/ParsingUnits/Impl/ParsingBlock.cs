using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.ParsingUnits.Impl
{
    public class ParsingBlock : ParsingUnitImpl, IParsingBlock
    {
        #region Fields

        private IParsingUnit _head;
        private readonly HashSet<IParsingUnit> _owned;
        private List<IParsingUnit> _cachedOwned;

        private readonly Comparer<IParsingUnit> _comparer;

        #endregion

        #region Constructors

        public ParsingBlock()
        {
            _owned = new HashSet<IParsingUnit>();
            _comparer = Comparer<IParsingUnit>.Create((x, y) => this.Owning(x) - this.Owning(y));
        }

        public ParsingBlock(IParsingUnit head)
            : this()
        {
            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            this.Add(head);
            this.Head = head;
        }

        #endregion

        #region Private

        private void AddSingleUnit(IParsingUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            if (!(unit is ParsingUnitImpl parsingUnitImpl))
            {
                throw new NotImplementedException(); // todo
            }

            if (unit == this)
            {
                throw new NotImplementedException(); // todo
            }

            if (_owned.Contains(unit))
            {
                throw new NotImplementedException(); // todo
            }

            if (unit.Owner != null)
            {
                throw new NotImplementedException(); // todo
            }

            if (unit is IParsingBlock parsingBlock)
            {
                if (this.IsNestedInto(parsingBlock))
                {
                    throw new NotImplementedException(); // todo
                }
            }

            _owned.Add(unit);
            parsingUnitImpl.Owner = this;
        }

        private int Owning(IParsingUnit unit)
        {
            return this.Owns(unit) ? 1 : 0;
        }

        #endregion

        #region Overridden

        protected override IReadOnlyList<IParsingUnit> ProcessImpl(ITokenStream stream, IParsingContext context)
        {
            IReadOnlyList<IParsingUnit> challengers = new List<IParsingUnit>(new[] { this.Head });
            IReadOnlyList<IParsingUnit> emptyChallengers = new List<IParsingUnit>();
            var oldPosition = stream.Position;

            while (true)
            {
                if (challengers.Count == 0)
                {
                    stream.Position = oldPosition;
                    return null;
                }

                IReadOnlyList<IParsingUnit> nextChallengers = emptyChallengers;

                foreach (var challenger in challengers)
                {
                    var result = challenger.Process(stream, context);

                    if (result != null)
                    {
                        // processed; let's dispatch to other challengers.
                        if (result.Count == 0)
                        {
                            throw new NotImplementedException(); // todo: internal error
                        }
                        else if (result.Count == 1)
                        {
                            var nextUnit = result[0];
                            if (this.Owns(nextUnit))
                            {
                                nextChallengers = result;
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            // prefer owned before non-owned
                            nextChallengers = result
                                .OrderBy(x => x, _comparer)
                                .ToList();
                        }

                        break;
                    }
                }

                challengers = nextChallengers;
            }
        }

        protected override void OnBeforeFinalize()
        {
            if (_head == null)
            {
                throw new NotImplementedException(); // todo
            }
        }

        protected override void FinalizeUnitImpl()
        {
            foreach (var unit in _owned)
            {
                unit.FinalizeUnit();
            }

            _cachedOwned = _owned.ToList();
        }

        #endregion

        #region IParsingBlock Members

        public IParsingUnit Head
        {
            get => _head;
            set
            {
                this.CheckNotFinalized();

                if (value == null)
                {
                    throw new NotImplementedException(); // todo
                }

                if (_head != null)
                {
                    throw new NotImplementedException(); // todo
                }

                if (!_owned.Contains(value))
                {
                    throw new NotImplementedException(); // todo
                }

                _head = value;
            }
        }

        public void Add(params IParsingUnit[] units)
        {
            this.CheckNotFinalized();

            foreach (var unit in units)
            {
                this.AddSingleUnit(unit);
            }
        }

        public bool Owns(IParsingUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            return _owned.Contains(unit);
        }

        public IReadOnlyList<IParsingUnit> Owned => _cachedOwned ?? _owned.ToList();

        #endregion
    }
}
