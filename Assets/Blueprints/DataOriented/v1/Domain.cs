using System;
using System.Collections;
using System.Collections.Generic;

namespace Blueprints.DoD.v1
{
    // you should only need to know about something in the domain if it's being interacted with or it's handling it's own logic
    // therefore Dictionary<Type, Dictionary<object, DataSet>> does the trick
    // one can have many instances of the same type, all with unique DataSets
    public static class Domain
    {
        static Domain()
        {
            Data = new Dictionary<Type, IDictionary<object, IDataSet>>();
        }
        
        private static IDictionary<Type, IDictionary<object, IDataSet>> Data { get; }
        
        public static ICollection<Type> Keys
            => Data.Keys;

        public static ICollection<IDictionary<object, IDataSet>> Values
            => Data.Values;

        public static int Length
            => Data.Count;

        public static int Count<T>()
            => Data.TryGetValue(typeof(T), out var pair) ? pair.Count : default;

        public static IDataSet Get<T>(object key)
        {
            if (!Data.TryGetValue(typeof(T), out var pair)) 
                return default;
            
            return pair.TryGetValue(key, out var data) ? data : default;
        }

        public static void Add<T>(object key, IDataSet value)
        {
            if (Data.TryGetValue(typeof(T), out var pair))
                pair.TryAdd(key, value);
            else
            {
                Data.Add(typeof(T), new Dictionary<object, IDataSet>());
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