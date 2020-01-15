using TauCode.Parsing.Exceptions;
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
            var length = text.Length;

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
                        throw new LexingException("Unclosed string.", new Position(initialLine + lineShift, column));
                    }

                    c = text[index];

                    if (LexingHelper.IsCaretControl(c)) // todo: this check is redundant, since both CR and LF are checked in switch.
                    {
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
                                    throw new LexingException("Unclosed string.", new Position(initialLine + lineShift, column));
                                }

                                break;

                            case LexingHelper.LF:
                                index++;
                                lineShift++;
                                column = 0;
                                break;
                        }
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
            else
            {
                return null;
            }
        }
    }
}
