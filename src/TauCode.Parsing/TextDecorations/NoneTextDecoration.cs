﻿namespace TauCode.Parsing.TextDecorations
{
    public class NoneTextDecoration : ITextDecoration
    {
        public static readonly NoneTextDecoration Instance = new NoneTextDecoration();

        private NoneTextDecoration()
        {
        }
    }
}
