using System;
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

        public string GetBlockName()
        {
            return this.Arguments[1]; // todo checks
        }

        public bool IsTop()
        {
            var topness = this.Arguments[0];

            switch (topness)
            {
                case "top":
                    return true;

                case "inner":
                    return false;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region IAideResult Members

        public string Name { get; }

        public IList<string> Arguments { get; }

        #endregion
    }
}
