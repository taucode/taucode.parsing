using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Data
{
    public class PrimaryKeyInfo
    {
        public string Name { get; set; }
        public List<IndexColumnInfo> Columns { get; set; } = new List<IndexColumnInfo>();
    }
}
