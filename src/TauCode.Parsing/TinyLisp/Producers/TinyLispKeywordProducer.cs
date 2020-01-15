using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispKeywordProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (c == ':')
            {
                var nameCharsCount = 0;
                var initialIndex = context.Index;
                var index = initialIndex + 1;
                var column = context.Column + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];

                    if (c == ':')
                    {
                        ThrowBadKeywordException(context.Line, context.Column);
                    }

                    if (!TinyLispHelper.IsAcceptableSymbolNameChar(c))
                    {
                        break;
                    }

                    nameCharsCount++;
                    index++;
                    column++;
                }

                if (nameCharsCount == 0)
                {
                    ThrowBadKeywordException(context.Line, context.Column);
                }

                var delta = index - initialIndex;
                var keywordName = text.Substring(initialIndex, delta);
                var token = new KeywordToken(
                    keywordName,
                    new Position(context.Line, context.Column), 
                    delta);
                context.Advance(delta, 0, column);
                return token;
            }
            else
            {
                return null;
            }
        }

        private static void ThrowBadKeywordException(int line, int column)
        {
            throw new LexingException("Bad keyword.", new Position(line, column));
        }
    }
}
