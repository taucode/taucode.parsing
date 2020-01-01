using System;
using TauCode.Extensions;
using TauCode.Parsing.Tokens;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextDecorations
{
    public class HyphenTextDecoration : ITextDecoration
    {
        public static readonly HyphenTextDecoration InstanceWithOneHyphen = new HyphenTextDecoration(1);
        public static readonly HyphenTextDecoration InstanceWithTwoHyphens = new HyphenTextDecoration(2);

        private HyphenTextDecoration(int hyphenCount)
        {
            if (!hyphenCount.IsIn(1, 2))
            {
                throw new ArgumentOutOfRangeException(nameof(hyphenCount));
            }

            this.HyphenCount = hyphenCount;
        }

        public int HyphenCount { get; }
    }
}
