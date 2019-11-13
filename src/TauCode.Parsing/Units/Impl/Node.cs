using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl
{
    // todo: clean up
    public abstract class Node : UnitImpl, INode
    {
        #region Fields

        private readonly List<IUnit> _links;
        //private readonly List<Link> _links;

        #endregion

        #region Constructor

        protected Node(string name)
            : base(name)
        {
            _links = new List<IUnit>();
            //this.Name = name;
            //_links = new List<Link>();
        }

        #endregion

        #region Overridden

        protected override void OnBeforeFinalize()
        {
            throw new NotImplementedException(); // resolve links!
            //if (_links.Count == 0)
            //{
            //    throw new ParserException($"Cannot finalize node since it doesn't have links. {this.ToUnitDiagnosticsString()}");
            //}

            //foreach (var link in _links)
            //{
            //    if (link is INode node && node.IsBlockHeadNode() && link.Owner != this.Owner)
            //    {
            //        throw new ParserException($"Cannot link to other block's head node; link to block instead. {this.ToUnitDiagnosticsString()}");
            //    }
            //}
        }

        #endregion

        #region Internal

        /// <summary>
        /// Adds a link regardless the fact the node is finalized
        /// </summary>
        /// <param name="linked">Unit to add link to.</param>
        internal void ForceAddLink(IUnit linked)
        {
            throw new NotImplementedException();

            //if (linked == null)
            //{
            //    throw new ArgumentNullException(nameof(linked));
            //}

            //// NB: can add self, no problem with that.
            //_links.Add(linked);
        }

        #endregion

        #region INode Members

        //public virtual void AddLink(IUnit linked)
        //{
        //    this.CheckNotFinalized();
        //    this.ForceAddLink(linked);
        //}

        //public IReadOnlyList<IUnit> Links => _links;

        public void AddLink(IUnit unit)
        {
            throw new NotImplementedException();
        }

        public void AddLinkAddress(string linkAddress)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<IUnit> Links => throw new NotImplementedException();


        #endregion
    }
}
