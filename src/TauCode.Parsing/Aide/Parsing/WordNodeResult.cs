namespace TauCode.Parsing.Aide.Parsing
{
    public class WordNodeResult : NodeResult
    {
        public WordNodeResult(string word)
        {
            this.Word = word;
        }

        public string Word { get; }
    }
}
