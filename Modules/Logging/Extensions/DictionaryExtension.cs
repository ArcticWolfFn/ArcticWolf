using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.Extensions
{
    public static class DictionaryExtension
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> target, Dictionary<TKey, TValue> source)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (KeyValuePair<TKey, TValue> element in source)
            {
                if (!target.ContainsKey(element.Key))
                {
                    target.Add(element.Key, element.Value);
                }
                else
                {
                    target.Remove(element.Key);
                    target.Add(element.Key, element.Value);
                }
            }
        }
    }
}
