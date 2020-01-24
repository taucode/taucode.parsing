using System.Collections.Generic;

namespace TauCode.Parsing.Tests.Parsing.Sql.Data
{
    public class ForeignKeyInfo
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public List<string> ColumnNames { get; set; } = new List<string>();
        public List<string> ReferencedColumnNames { get; set; } = new List<string>();
    }
}
