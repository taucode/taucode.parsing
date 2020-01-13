using TauCode.Parsing.TextProcessing;

namespace TauCode.Parsing.Tests.Lexing
{
    public struct CharPayload : IPayload
    {
        public CharPayload(char c)
        {
            this.Char = c;
        }

        public char Char { get; }
    }

    public class CLangEscapeProcessor : TextProcessorBase
    {
        public override bool AcceptsFirstChar(char c) => c == '\\';

        public override TextProcessingResult Process(ITextProcessingContext context)
        {
            var nextChar = context.TryGetNextChar();
            if (nextChar.HasValue)
            {
                switch (nextChar.Value)
                {
                    case 'n':
                        return new TextProcessingResult(2, 0, context.Column + 2, new CharPayload('\n')); // todo baaaaaaaaad
                    case 'r':
                        return new TextProcessingResult(2, 0, context.Column + 2, new CharPayload('\r')); // todo baaaaaaaaad
                }
            }

            return TextProcessingResult.Failure;
        }
    }
}
