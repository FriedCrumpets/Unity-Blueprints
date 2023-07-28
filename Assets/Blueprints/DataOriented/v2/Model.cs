using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Blueprints.DoD.v2
{
    public interface IModel : INotifyPropertyChanged
    {
        T Read<T>(string key);
        bool Set<T>(string key, T value);
        void Add<T>(string key, IData<T> value);
        bool Remove(string key);
    }

    [Serializable]
    public struct Model : IModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        private Dictionary<string, object> Data { get; }

        public Model(params KeyValuePair<string, object>[] data)
        {
            Data = new Dictionary<string, object>();

            foreach (var pair in data)
            {
                Data.Add(pair.Key, pair.Value);
            }

            PropertyChanged = (sender, args) => { };
        }

        T IModel.Read<T>(string key)
            => Data.TryGetValue(key, out var data) ? ((IData<T>)data).Get() : default;
        
        bool IModel.Set<T>(string key, T value)
        {
            if(Data.TryGetValue(key, out var data) is bool check);
            {
                ((IData<T>)data)?.Set(value);
                OnPropertyChanged(key);
            }

            return check;
        }
        
        void IModel.Add<T>(string key, IData<T> value)
            => Data.Add(key, value);

        bool IModel.Remove(string key)
            => Data.Remove(key);
        
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}