using System;
using System.Collections.Generic;

namespace Blueprints.DoD
{
    public interface IDataSet
    {
        T Read<T>(string key);
        IData<T> Get<T>(string key);
        bool Set<T>(string key, T value);
        void Add<T>(string key, IData<T> value);
        bool Remove(string key);
    } 
    
    public interface IDataSet<T>
    {
        IDataSet Data { get; }
        T Read(string key);
        IData<T> Get(string key);
        bool Set(string key, T value);
        void Add(string key, IData<T> value);
        bool Remove(string key);
    } 
    
    [Serializable]
    public class DataSet : IDataSet
    {
        private Dictionary<string, object> Data { get; }
        
        public DataSet(params KeyValuePair<string, object>[] data)
        {
            Data = new Dictionary<string,  object>();
            
            foreach (var pair in data)
            {
                Data.Add(pair.Key, pair.Value);
            }
        }
        
        T IDataSet.Read<T>(string key)
        {
            if (Data.TryGetValue(key, out var data))
            {
                return ((IData<T>)data).Get();
            }

            return default;
        }

        IData<T> IDataSet.Get<T>(string key)
        {
            if (Data.TryGetValue(key, out var data))
            {
                return (IData<T>)data;
            }

            return default;
        }

        bool IDataSet.Set<T>(string key, T value)
        { 
            if (Data.TryGetValue(key, out var data))
            {
               ((IData<T>)data).Set(value);
               return true;
            }

            return false;
        }

        void IDataSet.Add<T>(string key, IData<T> value)
            => Data.Add(key, value);

        bool IDataSet.Remove(string key)
            => Data.Remove(key);
    }

    public class DataSet<T> : IDataSet<T>
    {
        public DataSet()
        {
            Data = new DataSet();
        }
        
        public IDataSet Data { get; }

        T IDataSet<T>.Read(string key)
            => Data.Read<T>(key);

        IData<T> IDataSet<T>.Get(string key)
            => Data.Get<T>(key);

        bool IDataSet<T>.Set(string key, T value)
            => Data.Set(key, value);

        void IDataSet<T>.Add(string key, IData<T> value)
            => Data.Add(key, value);

        bool IDataSet<T>.Remove(string key)
            => Data.Remove(key);
    }
}