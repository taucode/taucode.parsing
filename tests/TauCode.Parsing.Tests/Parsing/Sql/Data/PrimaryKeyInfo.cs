using System.Collections.Generic;
using System.Text;

namespace TauCode.Parsing.Tests.Parsing.Sql.Data
{
    public class PrimaryKeyInfo
    {
        public string Name { get; set; }
        public List<IndexColumnInfo> Columns { get; set; } = new List<IndexColumnInfo>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"    CONSTRAINT [{this.Name}] PRIMARY KEY(");

            for (var i = 0; i < this.Columns.Count; i++)
            {
                var indexColumn = this.Columns[i];
                sb.Append($"[{indexColumn.ColumnName}] {indexColumn.SortDirection.ToString().ToUpperInvariant()}");

                if (i < this.Columns.Count - 1)
                {
                    sb.Append(", ");
                }
            }

            sb.Append(")");
            return sb.ToString();
        }
    }
}
