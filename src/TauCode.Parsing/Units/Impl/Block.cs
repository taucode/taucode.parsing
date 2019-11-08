using System;
using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing.Units.Impl
{
    public class Block : UnitImpl, IBlock
    {
        #region Fields

        private IUnit _head;
        private readonly HashSet<IUnit> _owned;
        private List<IUnit> _cachedOwned;

        private readonly Comparer<IUnit> _comparer;

        #endregion

        #region Constructors

        public Block()
        {
            _owned = new HashSet<IUnit>();
            _comparer = Comparer<IUnit>.Create((x, y) => this.Owning(x) - this.Owning(y));
        }

        public Block(IUnit head)
            : this()
        {
            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            this.Capture(head);
            this.Head = head;
        }

        #endregion

        #region Private

        private void AddSingleUnit(IUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            if (!(unit is UnitImpl unitImpl))
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

            if (unit is IBlock parsingBlock)
            {
                if (this.IsNestedInto(parsingBlock))
                {
                    throw new NotImplementedException(); // todo
                }
            }

            _owned.Add(unit);
            unitImpl.Owner = this;
        }

        private int Owning(IUnit unit)
        {
            return this.Owns(unit) ? 1 : 0;
        }

        #endregion

        #region Overridden

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            IReadOnlyList<IUnit> challengers = new List<IUnit>(new[] { this.Head });
            IReadOnlyList<IUnit> emptyChallengers = new List<IUnit>();
            var oldPosition = stream.Position;
            var oldVersion = context.Version;

            while (true)
            {
                if (challengers.Count == 0)
                {
                    if (context.Version == oldVersion)
                    {
                        stream.Position = oldPosition;
                        return null;
                    }
                    else
                    {
                        var debug = stream.GetCurrentToken();
                        throw new NotImplementedException(); // todo: context modified, but failed to end parsing => something bad happened.
                    }
                }

                IReadOnlyList<IUnit> nextChallengers = emptyChallengers;

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

        #region IBlock Members

        public IUnit Head
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

        public void Capture(params IUnit[] units)
        {
            this.CheckNotFinalized();

            foreach (var unit in units)
            {
                this.AddSingleUnit(unit);
            }
        }

        public bool Owns(IUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            return _owned.Contains(unit);
        }

        public IReadOnlyList<IUnit> Owned => _cachedOwned ?? _owned.ToList();

        public INode GetSingleExitNode()
        {
            return _owned.Where(x => x is Node).Cast<Node>().Single(x => x.Links.Count == 0);
        }

        #endregion
    }
}
