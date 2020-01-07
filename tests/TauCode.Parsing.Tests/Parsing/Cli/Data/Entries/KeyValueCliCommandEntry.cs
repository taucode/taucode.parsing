namespace TauCode.Parsing.Tests.Parsing.Cli.Data.Entries
{
    public class KeyValueCliCommandEntry : ICliCommandEntry
    {
        public string Alias { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
