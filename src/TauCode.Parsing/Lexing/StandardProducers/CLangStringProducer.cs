using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.TextDecorations;
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

        public LexingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var text = context.Text;
            var length = context.Length;

            var c = text[context.Index];

            if (c == '"')
            {
                var initialIndex = context.Index;
                var initialLine = context.Line;

                var index = initialIndex + 1; // skip '"'

                int delta;
                var sb = new StringBuilder();

                while (true)
                {
                    if (index == length)
                    {
                        var column = context.Column + (index - initialIndex);
                        throw LexingHelper.CreateUnclosedStringException(new Position(
                            initialLine,
                            column));
                    }

                    c = text[index];

                    if (LexingHelper.IsCaretControl(c))
                    {
                        var column = context.Column + (index - initialIndex);
                        throw LexingHelper.CreateNewLineInStringException(new Position(initialLine, column));
                    }

                    if (c == '\\')
                    {
                        if (index + 1 == length)
                        {
                            throw LexingHelper.CreateUnclosedStringException(new Position(
                                initialLine,
                                length));
                        }

                        var nextChar = text[index + 1];
                        if (nextChar == 'u')
                        {
                            var remaining = length - (index + 1);
                            if (remaining < 5)
                            {
                                delta = index - initialIndex;
                                var column = context.Column + delta;
                                this.ThrowBadEscapeException(initialLine, column);
                            }

                            var hexNumString = text.Substring(index + 2, 4);
                            var codeParsed = int.TryParse(
                                hexNumString,
                                NumberStyles.HexNumber,
                                CultureInfo.InvariantCulture,
                                out var code);

                            if (!codeParsed)
                            {
                                delta = index - initialIndex;
                                var column = context.Column + delta;
                                this.ThrowBadEscapeException(initialLine, column);
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
                                delta = index - initialIndex;
                                var column = context.Column + delta;
                                this.ThrowBadEscapeException(initialLine, column);
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

                delta = index - initialIndex;
                var str = sb.ToString();

                var token = new TextToken(
                    StringTextClass.Instance,
                    DoubleQuoteTextDecoration.Instance,
                    str,
                    new Position(context.Line, context.Column),
                    delta);

                context.Advance(delta, 0, context.Column + delta);
                return token;
            }

            return null;
        }

        private void ThrowBadEscapeException(int line, int column)
        {
            throw new LexingException("Bad escape.", new Position(line, column));
        }
    }
}
