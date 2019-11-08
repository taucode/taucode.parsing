using System.Collections.Generic;

namespace TauCode.Parsing.ParsingUnits.Impl
{
    public class ParsingSplitter : ParsingUnitImpl, IParsingSplitter
    {
        private readonly List<IParsingUnit> _ways;

        public ParsingSplitter()
        {
            _ways = new List<IParsingUnit>();
        }

        protected override IReadOnlyList<IParsingUnit> ProcessImpl(ITokenStream stream, IParsingContext context)
        {
            // todo: sort _ways, so nodes like 'End' and (hypothetical) 'BlockEnd' go last; use a ctor sorting method for that.
            foreach (var way in _ways)
            {
                IReadOnlyList<IParsingUnit> result = way.Process(stream, context);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        public void AddWay(IParsingUnit way)
        {
            // todo checks
            _ways.Add(way);
        }
    }
}
