using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    // todo: ut along with 'CliDoubleQuoteStringProducer'
    public class CliSingleQuoteStringProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (c == '\'')
            {
                var initialIndex = context.Index;
                var initialLine = context.Line;

                var index = initialIndex + 1; // skip '''

                int delta;

                while (true)
                {
                    if (index == length)
                    {
                        delta = index - initialIndex;
                        var column = context.Column + delta;
                        // todo use 'CreateUnclosedStringException' and ut.
                        throw new LexingException("Un-closed string.", new Position(initialLine, column));
                    }

                    c = text[index];

                    if (LexingHelper.IsCaretControl(c))
                    {
                        delta = index - initialIndex;
                        var column = context.Column + delta;
                        throw LexingHelper.CreateNewLineInStringException(new Position(initialLine, column));
                    }

                    index++;

                    if (c == '\'')
                    {
                        break;
                    }
                }

                delta = index - initialIndex;
                var str = text.Substring(initialIndex + 1, delta - 2);

                var token = new TextToken(
                    StringTextClass.Instance,
                    SingleQuoteTextDecoration.Instance,
                    str,
                    new Position(context.Line, context.Column),
                    delta);

                context.Advance(delta, 0, context.Column + delta);
                return token;
            }

            return null;
        }
    }
}
