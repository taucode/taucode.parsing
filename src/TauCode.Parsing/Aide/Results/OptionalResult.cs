using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class OptionalResult : IAideResult
    {
        #region Constructor

        public OptionalResult(string name)
        {
            this.Name = name;
            this.OptionalContent = new Content(this);
            this.Arguments = new List<string>();
        }

        #endregion

        #region Public

        public Content OptionalContent { get; }

        #endregion

        #region IAideResult Members

        public string Name { get; }

        public List<string> Arguments { get; }

        #endregion
    }
}
