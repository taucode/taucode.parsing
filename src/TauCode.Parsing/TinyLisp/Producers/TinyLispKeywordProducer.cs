using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispKeywordProducer : ITokenProducer
    {
        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var c = context.GetCurrentChar();

            if (c == ':') // todo: test input ":"
            {
                var nameCharsCount = 0;
                var text = context.Text;
                var length = text.Length;
                var initialIndex = context.GetIndex();
                var index = initialIndex + 1;
                var column = context.Column + 1;

                while (true)
                {
                    if (index == length)
                    {
                        break;
                    }

                    c = text[index];
                    if (!TinyLispHelper.IsAcceptableSymbolNameChar(c)) // todo: test bad input ":key1:key2"
                    {
                        break;
                    }

                    nameCharsCount++;
                    index++;
                    column++;
                }

                if (nameCharsCount == 0)
                {
                    return null; // todo: actually, throw.
                }

                var delta = index - initialIndex;
                var keywordName = text.Substring(initialIndex, delta);
                var token = new KeywordToken(keywordName, context.GetCurrentPosition(), delta);
                context.Advance(delta, 0, column);
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
