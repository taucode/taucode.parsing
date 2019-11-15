using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IContext2
    {
        ITokenStream TokenStream { get; }
        void SetNodes(IReadOnlyCollection<INode2> nodes); // todo: (debug-)check no idle nodes there?
        IReadOnlyCollection<INode2> GetNodes();
        IResultAccumulator ResultAccumulator { get; }
    }
}
