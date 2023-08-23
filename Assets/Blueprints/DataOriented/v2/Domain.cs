using System;
using System.Collections;
using System.Collections.Generic;

namespace Blueprints.DoD.v2
{
    public static class Domain
    {
        static Domain()
        {
            Data = new Dictionary<Type, IDictionary<object, IModel>>();
        }
        
        private static IDictionary<Type, IDictionary<object, IModel>> Data { get; }
        
        public static ICollection<Type> Keys
            => Data.Keys;

        public static ICollection<IDictionary<object, IModel>> Values
            => Data.Values;

        public static int Length
            => Data.Count;

        public static int Count<T>()
            => Data.TryGetValue(typeof(T), out var pairs) ? pairs.Count : default;

        public static bool TryGet<T>(object key, out IModel model)
        {
            if (Data.TryGetValue(typeof(T), out var pairs))
            {
                if (pairs.TryGetValue(key, out var data))
                {
                    model = data;
                    return true;
                }
            }

            model = default;
            return false;
        }

        public static void Add<T>(object key, ref IModel value)
        {
            if (Data.TryGetValue(typeof(T), out var pair))
                pair.TryAdd(key, value);
            else
            {
                Data.Add(typeof(T), new Dictionary<object, IModel>());
                Data[typeof(T)].Add(key, value);
            }
        }
        
        public static bool Remove<T>(object key)
            => Data.TryGetValue(typeof(T), out var pair) && pair.Remove(key);

        public static bool Contains<T>()
            => Data.ContainsKey(typeof(T));

        public static bool Contains<T>(object key)
            => Data.TryGetValue(typeof(T), out var pair) && pair.ContainsKey(key);

        public static IEnumerator GetEnumerator()
            => Data.GetEnumerator();

        public static IEnumerator GetEnumerator<T>()
            => Data.ContainsKey(typeof(T)) ? Data[typeof(T)].GetEnumerator() : default(IEnumerator);
    }
}