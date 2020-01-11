using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Parsing.Lab;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.TextClasses;
using TauCode.Parsing.Tokens;
using TauCode.Parsing.Tokens.TextDecorations;

namespace TauCode.Parsing.Tests.Parsing.Sql.TokenExtractors
{
    public class SqlIdentifierExtractor : GammaTokenExtractorBase<TextToken>
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
        
        public override TextToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var shift = _openingDelimiter.HasValue ? 1 : 0;

            var str = text.Substring(absoluteIndex + shift, consumedLength - shift * 2);

            return new TextToken(
                IdentifierTextClass.Instance,
                NoneTextDecoration.Instance,
                str,
                position,
                consumedLength);
        }

        protected override void OnBeforeProcess()
        {
            // todo: temporary check that IsProcessing == FALSE, everywhere
            if (this.IsProcessing)
            {
                throw new NotImplementedException();
            }

            // todo: temporary check that LocalPosition == 1, everywhere
            if (this.Context.GetLocalIndex() != 1)
            {
                throw new NotImplementedException();
            }

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

        protected override bool AcceptsPreviousTokenImpl(IToken previousToken)
        {
            return
                previousToken is PunctuationToken; // todo make it tunable (use list of acceptable token types in ctor).
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                // todo: check IsProcessing == false, here & anywhere
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
                    throw new NotImplementedException(); // todo error unclosed identifier.
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
                            throw new NotImplementedException(); // error: unclosed identifier
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

                throw new NotImplementedException();

                //if (localIndex > 1 && c == )
                //{

                //}
            }

            if (_openingDelimiter.HasValue)
            {
                throw new NotImplementedException(); // todo error unclosed identifier.
            }

            return CharAcceptanceResult.Fail;
        }
    }
}
