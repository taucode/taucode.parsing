namespace TauCode.Parsing.Tests.Data
{
    public class ColumnInfo
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public int? Precision { get; set; }
        public int? Scale { get; set; }
        public bool IsNullable { get; set; } = true;
    }
}
