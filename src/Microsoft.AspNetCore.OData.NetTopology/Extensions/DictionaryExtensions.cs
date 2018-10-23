using System.Collections.Generic;

namespace Microsoft.AspNet.OData
{
    internal static class DictionaryExtensions
    {
        public static void EnsureEntry<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }
    }
}