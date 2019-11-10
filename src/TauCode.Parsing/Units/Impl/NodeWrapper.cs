using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl
{
    public class NodeWrapper : UnitImpl, INodeWrapper
    {
        #region Fields

        private INode _internalNode;
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
                throw new NotImplementedException();
            }

            if (_deferredLinks.Count == 0)
            {
                throw new NotImplementedException();
            }

            foreach (var deferredLink in _deferredLinks)
            {
                _internalNode.AddLink(deferredLink);
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

                _internalNode = value;
            }
        }

        public void AddDeferredLink(IUnit unit)
        {
            // todo: checks
            _deferredLinks.Add(unit);
        }

        #endregion
    }
}
