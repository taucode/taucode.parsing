using System.Text;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing
{
    public class EscapePayload : IPayload
    {
        public EscapePayload(char char0, char? char1)
        {
            this.Char0 = char0;
            this.Char1 = char1;
        }

        public char Char0 { get; }
        public char? Char1 { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(this.Char0);
            if (this.Char1.HasValue)
            {
                sb.Append(this.Char1.Value);
            }

            return sb.ToString();
        }
    }
}
