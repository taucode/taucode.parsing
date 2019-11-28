using System.Collections.Generic;

namespace TauCode.Parsing
{
    public static class ToDoExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out var result);
            return result;
        }
    }
}
