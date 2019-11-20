using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Data
{
    public class TableInfo
    {
        public string Name { get; set; }
        public List<ColumnInfo> Columns { get; } = new List<ColumnInfo>();
        public PrimaryKeyInfo PrimaryKey { get; set; }
        public List<ForeignKeyInfo> ForeignKeys { get; set; } = new List<ForeignKeyInfo>();
        public string LastConstraintName { get; set; }
    }
}
