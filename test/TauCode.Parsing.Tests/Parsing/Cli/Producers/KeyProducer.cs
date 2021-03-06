﻿using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Cli.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.Producers
{
    public class KeyProducer : ITokenProducer
    {
        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = text.Length;

            var c = text[context.Index];

            if (c == '-')
            {

                var initialIndex = context.Index;
                var index = initialIndex + 1;
                int delta;
                var gotGoodChars = false;

                while (true)
                {
                    if (index == length)
                    {
                        if (text[index - 1] == '-')
                        {
                            return null;
                        }

                        break;
                    }

                    c = text[index];
                    delta = index - initialIndex;

                    if (c == '-')
                    {
                        if (delta == 1)
                        {
                            index++;
                            continue;
                        }

                        if (delta == 2)
                        {
                            if (text[index - 1] == '-')
                            {
                                return null; // "---" at beginning
                            }

                            index++;
                            continue;
                        }

                        if (text[index - 1] == '-')
                        {
                            return null; // "--" inside key
                        }

                        index++;
                        continue;
                    }

                    if (LexingHelper.IsDigit(c) ||
                        LexingHelper.IsLatinLetter(c))
                    {
                        gotGoodChars = true;
                        index++;
                        continue;
                    }

                    if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c) || c == '=')
                    {
                        if (text[index - 1] == '-')
                        {
                            return null;
                        }

                        break;
                    }

                    return null;
                }

                if (!gotGoodChars)
                {
                    return null;
                }

                delta = index - initialIndex;
                var str = text.Substring(initialIndex, delta);
                var position = new Position(context.Line, context.Column);
                context.Advance(delta, 0, context.Column + delta);
                var token = new TextToken(
                    KeyTextClass.Instance,
                    NoneTextDecoration.Instance,
                    str,
                    position,
                    delta);

                return token;
            }

            return null;
        }
    }
}
