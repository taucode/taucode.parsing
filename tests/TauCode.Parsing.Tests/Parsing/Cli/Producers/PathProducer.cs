using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    public class PathProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var c = context.GetCurrentChar();

            if (IsPathFirstChar(c))
            {
                var text = context.Text;
                var length = text.Length;

                var initialIndex = context.GetIndex();
                var index = initialIndex + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (IsPathFirstChar(c) || c == ':')
                    {
                        index++;
                        continue;
                    }

                    if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
                    {
                        break;
                    }

                    return null;
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta);

                var position = context.GetCurrentPosition();
                context.Advance(delta, 0, context.Column + delta);

                var token = new TextToken(
                    PathTextClass.Instance,
                    NoneTextDecoration.Instance,
                    str,
                    position,
                    delta);

                return token;
            }

            return null;
        }

        private static bool IsPathFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c) ||
            c.IsIn('\\', '/', '.', '!', '~', '$', '%', '-', '+');
    }
}
