using System;
using TauCode.Parsing.Units.Impl;

namespace TauCode.Parsing.Aide.Building
{
    public class UnitReference
    {
        public UnitReference(Node referencingNode, string holdingBlockName, string referencedUnitName)
        {
            this.ReferencingNode = referencingNode ?? throw new ArgumentNullException(nameof(referencingNode));
            this.HoldingBlockName = holdingBlockName ?? throw new ArgumentNullException(nameof(holdingBlockName));
            this.ReferencedUnitName = referencedUnitName ?? throw new ArgumentNullException(nameof(referencedUnitName));
        }

        public Node ReferencingNode { get; }
        public string HoldingBlockName { get; }
        public string ReferencedUnitName { get; }
    }
}
