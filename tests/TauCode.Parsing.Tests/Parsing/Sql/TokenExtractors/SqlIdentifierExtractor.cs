using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Exceptions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tests.Parsing.Sql.TextClasses;
using TauCode.Parsing.TextDecorations;
using TauCode.Parsing.TextProcessing;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    public class SqlIdentifierExtractor : TokenExtractorBase<TextToken>
    {
        private static Dictionary<char, char> Delimiters { get; }
        private static HashSet<char> OpeningDelimiters { get; }
        private static HashSet<char> ClosingDelimiters { get; }
        private static Dictionary<char, char> ReverseDelimiters { get; }

        static SqlIdentifierExtractor()
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

        private char? _openingDelimiter;

        public SqlIdentifierExtractor()
            : base(new[]
            {
                typeof(PunctuationToken)
            })
        {
        }

        protected override void OnBeforeProcess()
        {
            this.AlphaCheckOnBeforeProcess();

            var c = this.Context.GetLocalChar(0);
            if (OpeningDelimiters.Contains(c))
            {
                _openingDelimiter = c;
            }
            else
            {
                _openingDelimiter = null;
            }
        }

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            var shift = _openingDelimiter.HasValue ? 1 : 0;

            var str = text.Substring(absoluteIndex + shift, consumedLength - shift * 2);

            return new TextToken(
                SqlIdentifierClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                this.AlphaCheckNotBusyAndContextIsNull();
                // todo: check Context is null, here & anywhere.

                return this.ContinueOrFail(
                    OpeningDelimiters.Contains(c) ||
                    c == '_' ||
                    LexingHelper.IsLatinLetter(c));
            }

            if (c == '_' || LexingHelper.IsLatinLetter(c) || LexingHelper.IsDigit(c))
            {
                return CharAcceptanceResult.Continue;
            }

            if (LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                if (_openingDelimiter.HasValue)
                {
                    throw new LexingException("Unclosed identifier.", this.Context.GetCurrentAbsolutePosition());
                }
            }

            if (ClosingDelimiters.Contains(c))
            {
                // got closing delimiter.
                if (localIndex > 1)
                {
                    if (_openingDelimiter.HasValue)
                    {
                        if (_openingDelimiter.Value == ReverseDelimiters[c])
                        {
                            this.Context.AdvanceByChar();
                            return CharAcceptanceResult.Stop;
                        }
                        else
                        {
                            throw new LexingException("Unclosed identifier.", this.Context.GetCurrentAbsolutePosition());
                        }
                    }
                    else
                    {
                        return CharAcceptanceResult.Fail; // got closing delimiter without having opening.
                    }
                }
                else
                {
                    return CharAcceptanceResult.Fail; // got something like "[]" - delimited "empty" identifier
                }
            }

            if (_openingDelimiter.HasValue)
            {
                throw new LexingException("Unclosed identifier.", this.Context.GetCurrentAbsolutePosition());
            }

            return CharAcceptanceResult.Fail;
        }
    }
}
