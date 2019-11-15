using System.Collections.Generic;

namespace TauCode.Parsing.Aide.Results
{
    public class BlockDefinitionResult : IAideResult
    {
        #region Construcor

        public BlockDefinitionResult(string name)
        {
            this.Name = name;
            this.Content = new Content(this);
            this.Arguments = new List<string>();
        }

        #endregion

        #region Public
        
        public IContent Content { get; }

        #endregion

        #region IAideResult Members

        public string Name { get; }

        public IList<string> Arguments { get; }

        #endregion
    }
}
