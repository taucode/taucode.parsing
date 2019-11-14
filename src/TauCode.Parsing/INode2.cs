using System.Collections.Generic;

namespace TauCode.Parsing
{
    public interface INode2
    {
        string Name { get; }
        InquireResult Inquire(IToken token);
        void Consume(IToken token);
        void AddLink(INode2 node);
        IReadOnlyCollection<INode2> Links { get; }
    }
}
