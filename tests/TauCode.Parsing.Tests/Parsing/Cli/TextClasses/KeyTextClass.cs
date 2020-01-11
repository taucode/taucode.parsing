﻿using TauCode.Parsing.Lab;

namespace TauCode.Parsing.Tests.Parsing.Cli.TextClasses
{
    [TextClass("key")]
    public class KeyTextClass : TextClassBaseLab
    {
        public static readonly KeyTextClass Instance = new KeyTextClass();

        private KeyTextClass()
        {
        }

        protected override string TryConvertFromImpl(string text, ITextClassLab anotherClass)
        {
            throw new System.NotImplementedException();
        }
    }
}