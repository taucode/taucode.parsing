using System.Collections.Generic;
using System.Linq;

namespace TauCode.Parsing
{
    public static class ToDoExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out var result);
            return result;
        }

        public static void AddCharRange(this List<char> list, char from, char to)
        {
            // todo: check ranges and forth.

            list.AddRange(Enumerable.Range(from, to - from + 1).Select(x => (char)x));
        }
    }
}
