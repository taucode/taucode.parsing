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
            if (this.Arguments.Count != 2)
            {
                throw new AideException("Argument count is expected to be 2.");
            }

            return this.Arguments[1];
        }

        public bool IsTop()
        {
            if (this.Arguments.Count != 2)
            {
                throw new AideException("Argument count is expected to be 2.");
            }

            var topness = this.Arguments[0];

            switch (topness)
            {
                case "top":
                    return true;

                case "inner":
                    return false;

                default:
                    throw new AideException($"Invalid block modifier: '{topness}'.");
            }
        }

        #endregion

        #region IAideResult Members

        public string Name { get; }

        public List<string> Arguments { get; }

        #endregion
    }
}
