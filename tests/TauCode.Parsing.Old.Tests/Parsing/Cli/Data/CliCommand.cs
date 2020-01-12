using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Parsing.Cli.Data
{
    public class CliCommand
    {
        public string AddInName { get; set; }
        public string WorkerName { get; set; }
        public IList<ICliCommandEntry> Entries { get; set; } = new List<ICliCommandEntry>();
    }
}
