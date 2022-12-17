using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

namespace Collections
{
    [Serializable]
    public class Dict<TKey, TValue> : IList<KeyValue<TKey, TValue>>
    {
        public Dict()
        {
            Items = new List<KeyValue<TKey, TValue>>();
        }
        
        public Dict(IEnumerable<KeyValue<TKey, TValue>> items)
        {
            Items = items.ToList();
        }

        public Dict(IList<TKey> keys, IList<TValue> values)
        {
            if (!keys.Count.Equals(values.Count))
            {
                throw new ArgumentException($"Cannot build dictionary using lists of differing sizes");
            }

            Items = keys.Zip(values, (k, v) => new KeyValue<TKey, TValue>(k, v)).ToList();
        }

        [field: SerializeField] public List<KeyValue<TKey, TValue>> Items { get; private set; }
        
        public IEnumerator<KeyValue<TKey, TValue>> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValue<TKey, TValue> item) => Items.Add(item);

        public void Clear() => Items.Clear();

        public bool Contains(KeyValue<TKey, TValue> item) => Items.Contains(item);

        public void CopyTo(KeyValue<TKey, TValue>[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);

        public bool Remove(KeyValue<TKey, TValue> item) => Items.Remove(item);

        public int Count => Items.Count;

        public bool IsReadOnly => TypeDescriptor.GetDefaultProperty(this).IsReadOnly;

        public void Add(TKey key, TValue value) => Items.Add(new KeyValue<TKey, TValue>(key, value));

        public bool ContainsKey(TKey key) => Items.Any(item => item.Key.Equals(key));

        public bool Remove(TKey key)
        {
            var itemToRemove = Items.FirstOrDefault(item => item.Key.Equals(key));
            return Items.Remove(itemToRemove);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var item = Items.First(item => item.Key.Equals(key));

            var contained = Items.Contains(item);

            value = contained ? item.Value : default;
            
            return contained;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out var value))
                {
                    throw new KeyNotFoundException($"{key} not found in Dict: {this}");
                }

                return value;
            }
            set
            {
                if (!ContainsKey(key))
                {
                    throw new KeyNotFoundException($"{key} not found in Dict: {this}");
                }

                var item = Items.First(item => item.Key.Equals(key));
                var index = Items.IndexOf(item);
                Items[index] = new KeyValue<TKey, TValue>(key, value);
            }
        }

        public ICollection<TKey> Keys => Items.Select(item => item.Key).ToArray();
        
        public ICollection<TValue> Values => Items.Select(item => item.Value).ToArray();
        
        public int IndexOf(KeyValue<TKey, TValue> item) => Items.IndexOf(item);

        public void Insert(int index, KeyValue<TKey, TValue> item) => Items.Insert(index, item);

        public void RemoveAt(int index) => Items.RemoveAt(index);

        public KeyValue<TKey, TValue> this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
    }
    
    [Serializable]
    public class KeyValue<TKey, TValue> 
    {
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public KeyValue(KeyValue<TKey, TValue> item)
        {
            Key = item.Key;
            Value = item.Value;
        }

        [field: SerializeField] public TKey Key { get; set; }

        [field: SerializeField] public TValue Value { get; set; }

        public void Deconstruct(out TKey key, out TValue value)
        {
            key = Key;
            value = Value;
        }

        public override string ToString()
        {
            return $"Key:\t{Key}\r\nValue:\t{Value}";
        }
    }
}