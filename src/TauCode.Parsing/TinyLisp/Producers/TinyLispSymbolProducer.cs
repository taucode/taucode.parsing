using System;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispSymbolProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var c = this.Context.GetCurrentChar();

            if (c.IsAcceptableSymbolNameChar())
            {
                var gotSign = c == '+' || c == '-';
                var pureDigits = 0;
                if (!gotSign)
                {
                    pureDigits = LexingHelper.IsDigit(c) ? 1 : 0;
                }

                var gotNonDigits = false;
                
                
                var context = this.Context;

                var initialIndex = context.GetIndex();
                var initialColumn = context.Column;

                var index = initialIndex + 1;
                var column = context.Column + 1;
                var text = context.Text;
                var length = text.Length;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (!c.IsAcceptableSymbolNameChar()) // todo: test bad input "my-symbol:my-key"
                    {
                        break;
                    }

                    if (LexingHelper.IsDigit(c))
                    {
                        if (!gotNonDigits)
                        {
                            pureDigits++;
                        }
                    }
                    else
                    {
                        gotNonDigits = true;
                        pureDigits = 0;
                    }

                    index++;
                    column++;
                }

                var couldBeInt = pureDigits > 0;

                if (couldBeInt)
                {
                    throw new NotImplementedException(); // todo
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta);
                var symbolToken = new LispSymbolToken(str, new Position(context.Line, initialColumn), delta);
                context.Advance(delta, 0, column);
                return symbolToken;
            }
            else
            {
                return null;
            }
        }
    }
}
