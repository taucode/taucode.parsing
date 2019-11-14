//using System;
//using System.Collections.Generic;
//using TauCode.Parsing.Exceptions;

//namespace TauCode.Parsing.Units.Impl
//{
//    // todo: clean up
//    public abstract class Node : UnitImpl, INode
//    {
//        #region Fields

//        private readonly HashSet<IUnit> _linksChecked;
//        private readonly List<IUnit> _links;
//        private readonly HashSet<string> _linkAddresses;

//        #endregion

//        #region Constructor

//        protected Node(string name)
//            : base(name)
//        {
//            _links = new List<IUnit>();
//            _linksChecked = new HashSet<IUnit>();
//            _linkAddresses = new HashSet<string>();
//        }

//        #endregion

//        #region Overridden

//        protected override void OnBeforeFinalize()
//        {
//            if (_linkAddresses.Count != 0)
//            {
//                throw new NotImplementedException();
//            }

//            if (_links.Count == 0)
//            {
//                throw new ParserException($"Cannot finalize node since it doesn't have links. {this.ToUnitDiagnosticsString()}");
//            }

//            foreach (var link in _links)
//            {
//                if (link is INode node && node.IsBlockHeadNode() && link.Owner != this.Owner)
//                {
//                    throw new ParserException($"Cannot link to other block's head node; link to block instead. {this.ToUnitDiagnosticsString()}");
//                }
//            }
//        }

//        #endregion

//        #region Internal

//        /// <summary>
//        /// Adds a link regardless the fact the node is finalized
//        /// </summary>
//        /// <param name="unit">Unit to add link to.</param>
//        internal void ForceAddLink(IUnit unit)
//        {
//            if (unit == null)
//            {
//                throw new ArgumentNullException(nameof(unit));
//            }

//            if (_linksChecked.Contains(unit))
//            {
//                throw new NotImplementedException();
//            }

//            // NB: can add self, no problem with that.
//            _links.Add(unit);
//            _linksChecked.Add(unit);
//        }

//        #endregion

//        #region INode Members

//        public virtual void AddLink(IUnit unit)
//        {
//            this.CheckNotFinalized();
//            this.ForceAddLink(unit);
//        }

//        public virtual void AddLinkAddress(string linkAddress)
//        {
//            throw new NotImplementedException();
//        }

//        public IReadOnlyCollection<IUnit> Links => _links;

//        public IReadOnlyCollection<string> LinkAddresses => _linkAddresses;

//        #endregion
//    }
//}
