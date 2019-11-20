using System.Text;

namespace TauCode.Parsing.Tests.Data
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsNullable { get; set; } = true;
        public bool IsPrimaryKey { get; set; }
        public string Default { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"    [{this.Name}] {this.TypeName}");

            if (this.IsNullable)
            {
                sb.Append(" NULL");
            }
            else
            {
                sb.Append(" NOT NULL");
            }

            if (this.IsPrimaryKey)
            {
                sb.Append(" PRIMARY KEY");
            }

            if (this.Default != null)
            {
                sb.Append($" DEFAULT {this.Default}");
            }

            return sb.ToString();
        }
    }
}
