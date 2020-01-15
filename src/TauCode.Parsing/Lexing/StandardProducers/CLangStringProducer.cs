using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Lexing.StandardProducers
{
    public class CLangStringProducer : ITokenProducer
    {
        private static readonly string[] ReplacementStrings =
        {
            "\"\"",
            "\\\\",
            "0\0",
            "a\a",
            "b\b",
            "f\f",
            "n\n",
            "r\r",
            "t\t",
            "v\v",
        };

        private static readonly Dictionary<char, char> Replacements;

        private char? GetReplacement(char escape)
        {
            if (Replacements.TryGetValue(escape, out var replacement))
            {
                return replacement;
            }

            return null;
        }

        static CLangStringProducer()
        {
            Replacements = ReplacementStrings
                .ToDictionary(
                    x => x.First(),
                    x => x.Skip(1).Single());
        }

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

                var sb = new StringBuilder();

                while (true)
                {
                    if (index == length)
                    {
                        var column = context.Column + (index - initialIndex);
                        throw new LexingException("Unclosed string.", new Position(initialLine, column));
                    }

                    c = text[index];

                    if (LexingHelper.IsCaretControl(c))
                    {
                        throw new NotImplementedException(); // newline i C lang string
                    }

                    if (c == '\\')
                    {
                        if (index + 1 == length)
                        {
                            throw new NotImplementedException(); // end after escape
                        }

                        var nextChar = text[index + 1];
                        if (nextChar == 'u')
                        {
                            var remaining = length - (index + 1);
                            if (remaining < 5)
                            {
                                throw new NotImplementedException();
                            }

                            var hexNumString = text.Substring(index + 2, 4);
                            var codeParsed = int.TryParse(
                                hexNumString,
                                NumberStyles.HexNumber,
                                CultureInfo.InvariantCulture,
                                out var code);

                            if (!codeParsed)
                            {
                                throw new NotImplementedException();
                            }

                            var unescapedChar = (char)code;
                            sb.Append(unescapedChar);

                            index += 6;
                            continue;
                        }
                        else
                        {
                            var replacement = GetReplacement(nextChar);
                            if (replacement.HasValue)
                            {
                                sb.Append(replacement);
                                index += 2;
                                continue;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }

                    index++;

                    if (c == '"')
                    {
                        break;
                    }

                    sb.Append(c);
                }

                var delta = index - initialIndex;
                var str = sb.ToString();

                var token = new TextToken(
                    StringTextClass.Instance,
                    DoubleQuoteTextDecoration.Instance,
                    str,
                    context.GetCurrentPosition(),
                    delta);

                context.Advance(delta, 0, context.Column + delta);
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}
