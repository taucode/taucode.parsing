using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Data
{
    public class IndexInfo
    {
        public string Name { get; set; }
        public bool IsUnique { get; set; }
        public string TableName { get; set; }
        public List<IndexColumnInfo> Columns { get; set; } = new List<IndexColumnInfo>();
        public bool IsFinalized { get; set; }
    }
}
