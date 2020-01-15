using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    public class TermProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }
        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (LexingHelper.IsDigit(c) ||
                LexingHelper.IsLatinLetter(c))
            {
                var initialIndex = context.Index;
                var index = initialIndex + 1;

                while (true)
                {
                    if (index == length)
                    {
                        if (text[index - 1] == '-')
                        {
                            return null;
                        }

                        break;
                    }

                    c = text[index];

                    if (c == '-')
                    {
                        if (text[index - 1] == '-')
                        {
                            return null; // two '-' in a row
                        }

                        index++;
                        continue;
                    }

                    if (c == '=' || LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
                    {
                        if (text[index - 1] == '-')
                        {
                            return null;
                        }

                        break;
                    }

                    var valid =
                        LexingHelper.IsDigit(c) ||
                        LexingHelper.IsLatinLetter(c);

                    if (!valid)
                    {
                        return null;
                    }

                    index++;
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta);

                var position = new Position(context.Line, context.Column);
                context.Advance(delta, 0, context.Column + delta);

                var token = new TextToken(
                    TermTextClass.Instance,
                    NoneTextDecoration.Instance,
                    str,
                    position,
                    delta);

                return token;
            }

            return null;
        }
    }
}
