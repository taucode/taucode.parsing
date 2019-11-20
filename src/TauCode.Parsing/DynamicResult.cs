using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TauCode.Data;

namespace TauCode.Parsing
{
    public class DynamicResult : DynamicObject
    {
        #region Fields

        private readonly IDictionary<string, object> _values;

        #endregion

        #region Constructor

        public DynamicResult(object original = null)
        {
            IDictionary<string, object> values;

            if (original is DynamicResult dynamicRow)
            {
                values = new ValueDictionary(dynamicRow.ToDictionary());
            }
            else
            {
                values = new ValueDictionary(original);
            }

            _values = values;
        }

        #endregion

        #region Overridden

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = binder.Name;
            _values[name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name;
            return _values.TryGetValue(name, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames() => _values.Keys;

        #endregion

        #region Public

        public IDictionary<string, object> ToDictionary() => _values;

        public string[] GetNames() => this.GetDynamicMemberNames().ToArray();

        public void SetValue(string name, object value)
        {
            _values[name] = value;
        }

        public object GetValue(string name)
        {
            return _values[name];
        }

        public object this[string propertyName]
        {
            get => this.GetValue(propertyName);
            set => this.SetValue(propertyName, value);
        }
        
        #endregion
    }
}
