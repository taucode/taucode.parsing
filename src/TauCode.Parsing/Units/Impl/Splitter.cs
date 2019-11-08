using System.Collections.Generic;

namespace TauCode.Parsing.Units.Impl
{
    public class Splitter : UnitImpl, ISplitter
    {
        private readonly List<IUnit> _ways;

        public Splitter()
        {
            _ways = new List<IUnit>();
        }

        protected override IReadOnlyList<IUnit> ProcessImpl(ITokenStream stream, IContext context)
        {
            // todo: sort _ways, so nodes like 'End' and (hypothetical) 'BlockEnd' go last; use a ctor sorting method for that.
            foreach (var way in _ways)
            {
                IReadOnlyList<IUnit> result = way.Process(stream, context);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        protected override void OnBeforeFinalize()
        {
            // todo: must be <count_of_ways> >= 2
        }

        public void AddWay(IUnit way)
        {
            this.CheckNotFinalized();

            // todo checks
            _ways.Add(way);
        }
    }
}
