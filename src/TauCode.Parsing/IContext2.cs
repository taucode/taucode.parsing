using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface IContext2
    {
        ITokenStream TokenStream { get; }
        void SetNodes(params INode2[] nodes);
        IReadOnlyCollection<INode2> GetNodes();
    }
}
