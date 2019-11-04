using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public abstract class NodeParsingUnit : IParsingUnit
    {
        #region Fields

        private readonly List<NodeParsingUnit> _nextNodes;

        #endregion

        #region Constructor

        protected NodeParsingUnit(Action<IToken, IParsingContext> processor)
        {
            _nextNodes = new List<NodeParsingUnit>();
            this.Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        #endregion

        #region Protected

        protected Action<IToken, IParsingContext> Processor { get; }

        #endregion

        #region Public

        public void AddNextNode(NodeParsingUnit nextNode)
        {
            _nextNodes.Add(nextNode ?? throw new ArgumentNullException(nameof(nextNode)));
        }

        #endregion

        #region IParsingUnit Members

        public abstract ParseResult Process(ITokenStream stream, IParsingContext context);

        public IReadOnlyList<IParsingUnit> GetNextUnits()
        {
            return _nextNodes;
        }

        #endregion
    }
}
