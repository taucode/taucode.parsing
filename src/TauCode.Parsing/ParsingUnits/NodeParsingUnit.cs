using System;
using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits
{
    public abstract class NodeParsingUnit : IParsingUnit
    {
        #region Fields

        private readonly List<IParsingUnit> _nextUnits;

        #endregion

        #region Constructor

        protected NodeParsingUnit(Action<IToken, IParsingContext> processor)
        {
            _nextUnits = new List<IParsingUnit>();
            this.Processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        #endregion

        #region Protected

        protected Action<IToken, IParsingContext> Processor { get; }

        protected IReadOnlyList<IParsingUnit> NextUnits => Checked(_nextUnits);

        private IReadOnlyList<IParsingUnit> Checked(List<IParsingUnit> units)
        {
            if (units == null)
            {
                throw new NotImplementedException();
            }

            if (units.Count == 0)
            {
                throw new NotImplementedException();
            }

            return units;
        }

        #endregion

        #region Public

        public void AddNextUnit(IParsingUnit nextUnit)
        {
            _nextUnits.Add(nextUnit ?? throw new ArgumentNullException(nameof(nextUnit)));
        }

        #endregion

        #region IParsingUnit Members

        public abstract IReadOnlyList<IParsingUnit> Process(ITokenStream stream, IParsingContext context);

        #endregion
    }
}
