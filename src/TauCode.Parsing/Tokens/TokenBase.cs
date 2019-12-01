using System;
using System.Collections.Generic;

namespace TauCode.Parsing.Tokens
{
    public abstract class TokenBase : IToken
    {
        #region Fields

        private readonly Dictionary<string, string> _properties;

        #endregion

        #region Constructor

        protected TokenBase(
            string name = null,
            IEnumerable<KeyValuePair<string, string>> properties = null)
        {
            this.Name = name;
            _properties = new Dictionary<string, string>();
            if (properties != null)
            {
                foreach (var keyValuePair in properties)
                {
                    var key = keyValuePair.Key ?? throw new ArgumentException("'properties' cannot hold null keys.", nameof(properties));
                    var value = keyValuePair.Value ?? throw new ArgumentException("'properties' cannot hold null values.", nameof(properties));

                    _properties.Add(key, value);
                }
            }
        }

        #endregion

        #region IToken Members

        public string Name { get; }
        public virtual bool HasPayload => true;
        public IReadOnlyDictionary<string, string> Properties => _properties;

        #endregion
    }
}
