using System;
using System.Collections.Generic;
using TauCode.Parsing.Exceptions;

namespace TauCode.Parsing.Units.Impl
{
    public class NodeWrapper : UnitImpl, INodeWrapper
    {
        #region Fields

        private Node _internalNode;
        private readonly List<IUnit> _deferredLinks;

        #endregion

        #region Constructor

        public NodeWrapper(string name)
            : base(name)
        {
            _deferredLinks = new List<IUnit>();
        }

        #endregion

        #region Overridden

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            return _internalNode.Process(stream, context);
        }

        protected override void OnBeforeFinalize()
        {
            if (_internalNode == null)
            {
                throw new ParserException($"Cannot finalize node wrapper. Internal node is null. {this.ToUnitDiagnosticsString()}");
            }

            if (_deferredLinks.Count == 0 && _internalNode.Links.Count == 0)
            {
                throw new ParserException($"Cannot finalize node wrapper: no links established. {this.ToUnitDiagnosticsString()}");
            }

            foreach (var deferredLink in _deferredLinks)
            {
                _internalNode.ForceAddLink(deferredLink);
            }
        }

        #endregion

        #region INodeWrapper Members

        public INode InternalNode
        {
            get => _internalNode;
            set
            {
                this.CheckNotFinalized();

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value is Node node)
                {
                    _internalNode = node;
                }
                else
                {
                    throw new ArgumentException($"'{nameof(value)}' must be of type '{typeof(Node).FullName}'");
                }
            }
        }

        public void AddDeferredLink(IUnit unit)
        {
            if (unit == null)
            {
                throw new ArgumentNullException(nameof(unit));
            }

            _deferredLinks.Add(unit);
        }

        #endregion
    }
}
