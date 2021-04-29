﻿using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TinyLisp.Tokens;

namespace TauCode.Parsing.TinyLisp.Producers
{
    public class TinyLispSymbolProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = context.Length;

            var c = text[context.Index];

            if (TinyLispHelper.IsAcceptableSymbolNameChar(c))
            {
                var gotSign = c == '+' || c == '-';
                var pureDigits = 0;
                if (!gotSign)
                {
                    pureDigits = LexingHelper.IsDigit(c) ? 1 : 0;
                }

                var gotNonDigits = false;

                var initialIndex = context.Index;
                var initialColumn = context.Column;

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
                        throw new LexingException("Bad symbol name.", new Position(context.Line, context.Column));
                    }

                    if (!TinyLispHelper.IsAcceptableSymbolNameChar(c))
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
                    throw new LexingException("Symbol producer delivered an integer.", new Position(context.Line, context.Column));
                }

                var delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta); // todo: use ReadOnlySpan<char> instead of Substring everywhere.
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
