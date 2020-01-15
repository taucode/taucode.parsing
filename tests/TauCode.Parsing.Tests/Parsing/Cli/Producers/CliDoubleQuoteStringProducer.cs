using System;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    public class CliDoubleQuoteStringProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (c == '\"')
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

                    if (LexingHelper.IsCaretControl(c))
                    {
                        throw new NotImplementedException(); // newline in constant
                    }

                    index++;
                    column++;

                    if (c == '\"')
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
