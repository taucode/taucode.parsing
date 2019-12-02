using System;

namespace TauCode.Parsing.Building
{
    public class BuildResult
    {
        public BuildResult(NodeBox head, NodeBox tail)
        {
            this.Head = head ?? throw new ArgumentNullException(nameof(head));
            this.Tail = tail ?? throw new ArgumentNullException(nameof(tail));
        }

        public NodeBox Head { get; }
        public NodeBox Tail { get; }
    }

}
