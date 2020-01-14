using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Omicron;
using TauCode.Parsing.Tests.Parsing.Sql.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.Producers
{
    public class SqlIdentifierProducer : IOmicronTokenProducer
    {
        private static Dictionary<char, char> Delimiters { get; }
        private static HashSet<char> OpeningDelimiters { get; }
        private static HashSet<char> ClosingDelimiters { get; }
        private static Dictionary<char, char> ReverseDelimiters { get; }

        static SqlIdentifierProducer()
        {
            Delimiters = new[]
            {
                "[]",
                "\"\"",
                "``",
            }
                .ToDictionary(x => x[0], x => x[1]);

            OpeningDelimiters = new HashSet<char>(Delimiters.Keys);
            ClosingDelimiters = new HashSet<char>(Delimiters.Values);

            ReverseDelimiters = Delimiters
                .ToDictionary(x => x.Value, x => x.Key);

        }

        public TextProcessingContext Context { get; set; }

        public IToken Produce()
        {
            var context = this.Context;
            var c = context.GetCurrentChar();

            if (OpeningDelimiters.Contains(c) ||
                c == '_' ||
                LexingHelper.IsLatinLetter(c))
            {
                char? openingDelimiter = OpeningDelimiters.Contains(c) ? c : (char?)null;

                var initialIndex = context.GetIndex();
                var index = initialIndex + 1;
                var column = context.Column + 1;

                var text = context.Text;
                var length = text.Length;

                while (true)
                {
                    if (index == length)
                    {
                        if (openingDelimiter.HasValue)
                        {
                            throw new NotImplementedException(); // unclosed identifier
                        }
                        break;
                    }

                    c = text[index];

                    if (c == '_' || LexingHelper.IsLatinLetter(c) || LexingHelper.IsDigit(c))
                    {
                        index++;
                        column++; // todo: can calculate this via index.
                        continue;
                    }

                    if (ClosingDelimiters.Contains(c))
                    {
                        if (index - initialIndex > 2)
                        {
                            if (openingDelimiter.HasValue)
                            {
                                if (openingDelimiter.Value == ReverseDelimiters[c])
                                {
                                    index++;
                                    column++;

                                    var delta = index - initialIndex;

                                    var str = text.Substring(initialIndex + 1, delta - 2);
                                    var position = context.GetCurrentPosition();
                                    context.Advance(delta, 0, column);
                                    return new TextToken(
                                        SqlIdentifierClass.Instance,
                                        NoneTextDecoration.Instance,
                                        str,
                                        position,
                                        delta);
                                }
                                else
                                {
                                    throw new NotImplementedException(); // unclosed identifier
                                }
                            }
                            else
                            {
                                throw new NotImplementedException(); // got closing delimiter without opening.
                            }
                        }
                        else
                        {
                            return null; // got something like "[]" - delimited "empty" identifier
                        }
                    }
                }

                throw new NotImplementedException();
            }

            return null;
        }
    }
}
