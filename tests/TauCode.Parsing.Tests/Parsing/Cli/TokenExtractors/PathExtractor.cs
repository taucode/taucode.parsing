﻿using System;
using TauCode.Extensions;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Tests.Parsing.Cli.TokenExtractors
{
    public class PathExtractor : TokenExtractorBase
    {
        public PathExtractor()
            : base(IsPathFirstChar)
        {
        }

        private static bool IsPathFirstChar(char c) =>
            LexingHelper.IsDigit(c) ||
            LexingHelper.IsLatinLetter(c) ||
            c.IsIn('\\', '/', '.', '!', '~', '$', '%', '-', '+');

        protected override void ResetState()
        {
            // idle
        }

        protected override IToken ProduceResult()
        {
            var str = this.ExtractResultString();
            throw new NotImplementedException();
            //var token = new TextToken(PathTextClass.Instance, NoneTextDecoration.Instance, str);
            //return token;
        }

        protected override CharChallengeResult ChallengeCurrentChar()
        {
            var c = this.GetCurrentChar();
            var pos = this.GetLocalPosition();

            if (pos == 0)
            {
                return CharChallengeResult.Continue; // 0th char MUST have been accepted.
            }

            if (IsPathFirstChar(c) || c == ':')
            {
                return CharChallengeResult.Continue;
            }

            throw new NotImplementedException();

            //if (this.Environment.IsSpace(c))
            //{
            //    return CharChallengeResult.Finish;
            //}

            //return CharChallengeResult.GiveUp;
        }

        protected override CharChallengeResult ChallengeEnd()
        {
            return CharChallengeResult.Finish;
        }
    }
}
