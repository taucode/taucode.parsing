using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Data
{
    public class TableInfo
    {
        public string Name { get; set; }

        public List<ColumnInfo> Columns { get; } = new List<ColumnInfo>();
    }
}
