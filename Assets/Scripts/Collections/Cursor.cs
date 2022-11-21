using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Collections
{
    public class Cursor<T> : IList<T>
    {
        public Cursor() { }

        public Cursor(IList<T> list) { Items = list; }
        
        private uint _location = uint.MinValue;

        public IList<T> Items { get; private set; }

        public uint Location
        {
            get => _location;
            set
            {
                if (value > Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(value),$"Attempting to set location above the Item count");
                }

                _location = value;
            }
        }
        
        public int Count => Items.Count;

        public bool IsReadOnly => Items.IsReadOnly;

        public T this[int index]
        {
            get
            {
                if (!Items.Any())
                {
                    throw new NullReferenceException($"There are no Items on this Cursor to retrieve");
                }

                Location = (uint)index;
                return Items.ElementAt(index);
            }
            set => throw new NotImplementedException();
        }
        
        public T CurrentItem
        {
            get
            {
                if (!Items.Any())
                {
                    throw new NullReferenceException($"There are no Items on this Cursor to retrieve");
                }

                return Items.ElementAt((int)Location - 1);
            }
        }

        public void Add(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Cannot add a null item to a Cursor");
            }
            
            Location++;
            Items.Add(item);
        }
        
        public bool Remove(T item)
        {
            if (Location != uint.MinValue)
            {
                Location--;
            }
            
            return Items.Remove(item);
        }

        public IList<T> Slice(int from, int to) => Items.ToArray()[from..^to];

        public void Clear() { Items.Clear(); }

        public void ClearFromLocation() => Items = Slice((int)Location, Count);

        public void ClearToLocation() => Items = Slice(0, (int)Location);

        public bool Contains(T item) => Items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item) => Remove(item);
        
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => Items.IndexOf(item);

        public void Insert(int index, T item)
        {
            Location++;
            Items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            if (Location != uint.MinValue)
            {
                Location--;
            }
            
            Items.RemoveAt(index);
        }
    }
}