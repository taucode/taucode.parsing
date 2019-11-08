using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl
{
    public abstract class Node : UnitImpl, INode
    {
        #region Fields

        private readonly List<IUnit> _links;

        #endregion

        #region Constructor

        protected Node(Action<IToken, IContext> processor)
        {
            _links = new List<IUnit>();
            this.Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        #endregion

        #region Protected

        protected Action<IToken, IContext> Processor { get; }

        #endregion

        #region Abstract

        protected abstract bool IsAcceptableToken(IToken token);

        #endregion

        #region Overridden

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            var token = stream.GetCurrentToken();
            if (this.IsAcceptableToken(token))
            {
                this.Processor(token, context);
                stream.AdvanceStreamPosition();
                return this.Links;
            }

            return null;
        }

        protected override void OnBeforeFinalize()
        {
            if (_links.Count == 0)
            {
                throw new NotImplementedException(); // todo: cannot finalize this.
            }

            foreach (var link in _links)
            {
                if (link is INode node && node.IsBlockHeadNode())
                {
                    throw new NotImplementedException(); // todo: cannot link to block's head node; link to block instead.
                }
            }
        }

        #endregion

        #region INode Members

        public virtual void AddLink(IUnit linked)
        {
            this.CheckNotFinalized();

            if (linked == null)
            {
                throw new ArgumentNullException(nameof(linked));
            }

            // NB: can add self, no problem with that.

            _links.Add(linked);
        }

        public IReadOnlyList<IUnit> Links => _links;

        #endregion
    }
}
