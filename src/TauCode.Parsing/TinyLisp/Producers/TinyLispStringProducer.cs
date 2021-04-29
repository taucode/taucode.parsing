using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispStringProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = context.Length;

            var c = text[context.Index];

            if (c == '"')
            {
                var initialIndex = context.Index;
                var initialLine = context.Line;

                var index = initialIndex + 1; // skip '"'
                var lineShift = 0;
                var column = context.Column + 1; // skip '"'

                while (true)
                {
                    if (index == length)
                    {
                        throw LexingHelper.CreateUnclosedStringException(new Position(initialLine + lineShift, column));
                    }

                    c = text[index];

                    switch (c)
                    {
                        case LexingHelper.CR:
                            index++;
                            lineShift++;
                            column = 0;

                            if (index < length)
                            {
                                var nextChar = text[index];
                                if (nextChar == LexingHelper.LF)
                                {
                                    index++;
                                }
                            }
                            else
                            {
                                throw LexingHelper.CreateUnclosedStringException(new Position(
                                    context.Line + lineShift,
                                    column));
                            }

                            continue;

                        case LexingHelper.LF:
                            index++;
                            lineShift++;
                            column = 0;
                            continue;
                    }

                    index++;
                    column++;

                    if (c == '"')
                    {
                        break;
                    }
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex + 1, delta - 2);

                var token = new TextToken(
                    StringTextClass.Instance,
                    DoubleQuoteTextDecoration.Instance,
                    str,
                    new Position(context.Line, context.Column),
                    delta);

                context.Advance(delta, lineShift, column);
                return token;
            }

            return null;
        }
    }
}
