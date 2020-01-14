using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.Omicron.Producers
{
    public class SymbolProducer : IOmicronTokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var c = this.Context.GetCurrentChar();

            if (c.IsAcceptableSymbolNameChar())
            {
                var couldBeInt = LexingHelper.IsIntegerFirstChar(c);

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

                    if (!c.IsAcceptableSymbolNameChar())
                    {
                        break;
                    }

                    if (!LexingHelper.IsDigit(c))
                    {
                        couldBeInt = false;
                    }

                    index++;
                    column++;
                }

                if (couldBeInt)
                {
                    return null;
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
