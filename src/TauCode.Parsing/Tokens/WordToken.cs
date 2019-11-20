﻿using System;

namespace TauCode.Parsing.Tokens
{
    public class WordToken : TokenBase
    {
        #region Constructor

        public WordToken(string word)
        {
            this.Word = word ?? throw new ArgumentNullException(nameof(word));
        }

        public WordToken(string word, string name)
            : base(name)
        {
            this.Word = word;
        }


        #endregion

        #region Public

        public string Word { get; }

        #endregion
    }
}
