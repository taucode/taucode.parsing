using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results2
{
    public class OptionalResult : IAideResult2
    {
        public OptionalResult()
        {
            this.OptionalContent = new Content();
        }

        public Content OptionalContent { get; }
        public IList<string> Arguments => throw new NotImplementedException();
    }
}
