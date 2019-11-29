﻿using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public class IntegerToken : TokenBase
    {
        #region Constructor

        public IntegerToken(
            string integerValue,
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
            : base(name, properties)
        {
            this.IntegerValue = integerValue ?? throw new ArgumentNullException(nameof(integerValue));
        }

        #endregion

        #region Public

        public string IntegerValue { get; }

        #endregion
    }
}
