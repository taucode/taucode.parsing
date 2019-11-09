namespace TauCode.Parsing.Aide.Parsing
{
    public class WordNodeResult : NodeResult
    {
        public WordNodeResult(string word, string tag)
            : base(tag)
        {
            this.Word = word;
        }

        public string Word { get; }
    }
}
