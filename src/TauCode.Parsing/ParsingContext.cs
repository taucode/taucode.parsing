using System.Collections.Generic;

namespace TauCode.Parsing
{
    public class ParsingContext : IParsingContext
    {
        private readonly Dictionary<string, DynamicResult> _results;

        public ParsingContext()
        {
            _results = new Dictionary<string, DynamicResult>();
        }

        public void Push(string objectName, dynamic properties)
        {
            var newObject = new DynamicResult();
            var dynamicProperties = new DynamicResult(properties);

            foreach (var propertyName in dynamicProperties.GetNames())
            {
                newObject.SetValue(propertyName, dynamicProperties.GetValue(propertyName));
            }

            _results.Add(objectName, newObject);
        }
    }
}
