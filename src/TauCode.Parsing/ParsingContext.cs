﻿using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing
{
    public class ParsingContext : IParsingContext
    {
        private readonly List<object> _results;

        public ParsingContext()
        {
            _results = new List<object>();
        }

        //private readonly Dictionary<string, dynamic> _results;

        //public ParsingContext()
        //{
        //    _results = new Dictionary<string, dynamic>();
        //}

        //public void Add(string objectName, dynamic properties)
        //{
        //    var newObject = new DynamicResult();
        //    var dynamicProperties = new DynamicResult(properties);

        //    foreach (var propertyName in dynamicProperties.GetNames())
        //    {
        //        newObject.SetValue(propertyName, dynamicProperties.GetValue(propertyName));
        //    }

        //    _results.Add(objectName, newObject);
        //}

        //public void Update(string objectName, dynamic properties)
        //{
        //    var existingObject = _results[objectName];
        //    var dynamicProperties = new DynamicResult(properties);
        //    foreach (var propertyName in dynamicProperties.GetNames())
        //    {
        //        existingObject.SetValue(propertyName, dynamicProperties.GetValue(propertyName));
        //    }
        //}

        //public dynamic Get(string objectName)
        //{
        //    return _results[objectName];
        //}

        //public void Remove(string objectName)
        //{
        //    _results.Remove(objectName);
        //}

        public void AddResult(object result)
        {
            _results.Add(result);
        }

        public T GetLastResult<T>()
        {
            return (T)_results.Last();
        }

        public object[] ToArray()
        {
            return _results.ToArray();
        }
    }
}
