using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Omicron.Producers
{
    public class StringProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

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
                var line = initialLine;
                var column = context.Column + 1; // skip '"'

                while (true)
                {
                    if (index == length)
                    {
                        throw new NotImplementedException(); // unclosed string.
                    }

                    c = text[index];

                    if (LexingHelper.IsCaretControl(c))
                    {
                        throw new NotImplementedException();
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

                context.Advance(delta, line - initialLine, column);
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
