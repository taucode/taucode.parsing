using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits.Impl
{
    public abstract class ParsingNode : ParsingUnitImpl, IParsingNode
    {
        #region Fields

        private readonly List<IParsingUnit> _links;

        #endregion

        #region Constructor

        protected ParsingNode(Action<IToken, IParsingContext> processor)
        {
            _links = new List<IParsingUnit>();
            this.Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        #endregion

        #region Protected

        protected Action<IToken, IParsingContext> Processor { get; }

        #endregion

        #region Abstract

        protected abstract bool IsAcceptableToken(IToken token);

        #endregion

        #region Overridden

        protected override IReadOnlyList<IParsingUnit> ProcessImpl(ITokenStream stream, IParsingContext context)
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
                if (link is IParsingNode node && node.IsBlockHeadNode())
                {
                    throw new NotImplementedException(); // todo: cannot link to block's head node; link to block instead.
                }
            }
        }

        #endregion

        #region IParsingNode Members

        public virtual void AddLink(IParsingUnit linked)
        {
            this.CheckNotFinalized();

            if (linked == null)
            {
                throw new ArgumentNullException(nameof(linked));
            }

            // NB: can add self, no problem with that.

            _links.Add(linked);
        }

        public IReadOnlyList<IParsingUnit> Links => _links;

        #endregion
    }
}
