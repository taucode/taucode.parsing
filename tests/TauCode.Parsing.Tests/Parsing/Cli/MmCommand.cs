using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Parsing.Cli
{
    public class MmCommand
    {
        public IList<MmCommandEntry> Entries { get; set; } = new List<MmCommandEntry>();
    }
}
