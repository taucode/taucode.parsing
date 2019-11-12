using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;

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

        private Block()
            : base(null)
        {
            _owned = new HashSet<IUnit>();
            _comparer = Comparer<IUnit>.Create((x, y) => this.Owning(x) - this.Owning(y));
        }

        public Block(string name)
            : this()
        {
            this.Name = name;
        }

        private Block(IUnit head)
            : this()
        {
            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            this.Capture(head);
            this.Head = head;
        }

        public Block(IUnit head, string name)
            : this(head)
        {
            this.Name = name;
        }

        #endregion

        #region Private

        private void CaptureSingleUnit(IUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            if (!(unit is UnitImpl unitImpl))
            {
                throw new ArgumentException($"Implementation can only capture descendants of {typeof(UnitImpl).FullName}. {this.ToUnitDiagnosticsString()}", nameof(unit));
            }

            if (unit == this)
            {
                throw new ArgumentException($"Cannot capture self. {this.ToUnitDiagnosticsString()}", nameof(unit));
            }

            if (_owned.Contains(unit))
            {
                throw new ArgumentException($"Already owns unit with name '{unit.Name}'. {this.ToUnitDiagnosticsString()}", nameof(unit));
            }

            if (unit.Owner != null)
            {
                throw new ArgumentException($"Owner of '{unit.Name}' is already set. {this.ToUnitDiagnosticsString()}", nameof(unit));
            }

            if (unit is IBlock parsingBlock)
            {
                if (this.IsNestedInto(parsingBlock))
                {
                    throw new ArgumentException($"Capturing will lead to circular nesting. {this.ToUnitDiagnosticsString()}", nameof(unit));
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
                        throw new ParserException($"Syntax error. {this.ToUnitDiagnosticsString()}");
                    }
                }

                if (ParsingHelper.IsEndResult(challengers))
                {
                    return challengers;
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
                            throw new ParserException($"Parsing logic error. Result of 'Process' is not null, but empty. {this.ToUnitDiagnosticsString()}");
                        }
                        else if (result.Count == 1)
                        {
                            var nextUnit = result[0];
                            if (nextUnit.IsNestedInto(this))
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
                throw new ParserException($"Head of block is null. {this.ToUnitDiagnosticsString()}");
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
                    throw new ParserException($"Cannot set block's head to null. {this.ToUnitDiagnosticsString()}");
                }

                if (_head != null)
                {
                    throw new ParserException($"Block's head is already set. {this.ToUnitDiagnosticsString()}");
                }

                if (!_owned.Contains(value))
                {
                    throw new ParserException($"Block doesn't own this unit. {this.ToUnitDiagnosticsString()}");
                }

                _head = value;
            }
        }

        public void Capture(params IUnit[] units)
        {
            this.CheckNotFinalized();

            foreach (var unit in units)
            {
                this.CaptureSingleUnit(unit);
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
