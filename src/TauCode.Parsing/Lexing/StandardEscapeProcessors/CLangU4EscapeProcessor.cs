using System.Globalization;
using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Lexing.StandardEscapeProcessors
{
    public class CLangU4EscapeProcessor : EscapeProcessorBase
    {
        public CLangU4EscapeProcessor()
            : base('\\', false)
        {
        }

        protected override EscapePayload DeliverPayloadImpl()
        {
            // I want 5 more chars: uXXXX
            var got5 = this.Context.RequestChars(5);

            if (!got5)
            {
                return null;
            }

            var c = this.Context.GetCurrentChar();
            if (c != 'u')
            {
                return null;
            }

            this.Context.AdvanceByChar();
            var hexNumber = this.Context.GetSubstring(4);

            if (!int.TryParse(hexNumber, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var n))
            {
                return null;
            }

            this.Context.AdvanceByChars(4);
            return new EscapePayload((char)n, null);
        }
    }
}
