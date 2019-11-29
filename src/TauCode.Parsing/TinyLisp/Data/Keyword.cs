namespace TauCode.Parsing.TinyLisp.Data
{
    public sealed class Keyword : Symbol
    {
        public Keyword(string name)
            : base(name, true)
        {
        }
    }
}
