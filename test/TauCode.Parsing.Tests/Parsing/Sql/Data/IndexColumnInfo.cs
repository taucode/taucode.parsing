namespace TauCode.Parsing.Tests.Parsing.Sql.Data
{
    public class IndexColumnInfo
    {
        public string ColumnName { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Asc;
    }

    public enum SortDirection
    {
        Asc = 1,
        Desc
    }
}
