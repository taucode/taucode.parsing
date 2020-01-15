using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispStringProducer : ITokenProducer
    {
        public ITextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var c = this.Context.GetCurrentChar();
            if (c == '"')
            {
                var text = context.Text;
                var length = text.Length;

                var initialIndex = context.GetIndex();
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

                    if (LexingHelper.IsCaretControl(c))
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

                            default:
                                throw new NotImplementedException(); // actually, cannot be.
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
                    context.GetCurrentPosition(),
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
